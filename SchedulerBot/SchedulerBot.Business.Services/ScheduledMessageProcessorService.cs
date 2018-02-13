using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SchedulerBot.Database.Core;

namespace SchedulerBot.Business.Services
{
	public sealed class ScheduledMessageProcessorService : IHostedService, IDisposable
	{
		private readonly CancellationTokenSource serviceCancellationTokenSource;
		private readonly CancellationToken serviceCancellationToken;
		private readonly IServiceScopeFactory scopeFactory;

		public ScheduledMessageProcessorService(IServiceScopeFactory scopeFactory)
		{
			this.scopeFactory = scopeFactory;
			serviceCancellationTokenSource = new CancellationTokenSource();
			serviceCancellationToken = serviceCancellationTokenSource.Token;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			while (!serviceCancellationTokenSource.IsCancellationRequested)
			{
				await ProcessScheduledMessagesAsync();
				await WaitAsync(serviceCancellationToken);
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			serviceCancellationTokenSource.Cancel();

			// TODO: make this wait for the real cancellation.
			return Task.CompletedTask;
		}

		public void Dispose()
		{
			serviceCancellationTokenSource?.Dispose();
		}

		private async Task ProcessScheduledMessagesAsync()
		{
			using (IServiceScope scope = scopeFactory.CreateScope())
			{
				SchedulerBotContext context = scope.ServiceProvider.GetRequiredService<SchedulerBotContext>();

				// TODO: The logic.

				await context.SaveChangesAsync(serviceCancellationToken);
			}
		}

		private static async Task WaitAsync(CancellationToken cancellationToken)
		{
			try
			{
				await Task.Delay(1000, cancellationToken);
			}
			catch (TaskCanceledException)
			{
				// Just swallow it.
			}
		}
	}
}
