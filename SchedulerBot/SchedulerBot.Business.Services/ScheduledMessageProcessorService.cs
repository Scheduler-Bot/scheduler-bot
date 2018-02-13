using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SchedulerBot.Database.Core;

namespace SchedulerBot.Business.Services
{
	public sealed class ScheduledMessageProcessorService : IHostedService, IDisposable
	{
		private readonly IServiceScopeFactory scopeFactory;
		private readonly TimeSpan pollingInterval;
		private readonly CancellationTokenSource serviceCancellationTokenSource;
		private readonly CancellationToken serviceCancellationToken;

		public ScheduledMessageProcessorService(IServiceScopeFactory scopeFactory, IConfiguration configuration)
		{
			this.scopeFactory = scopeFactory;
			pollingInterval = TimeSpan.Parse(configuration["MessageProcessingInterval"], CultureInfo.InvariantCulture);
			serviceCancellationTokenSource = new CancellationTokenSource();
			serviceCancellationToken = serviceCancellationTokenSource.Token;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			while (!serviceCancellationTokenSource.IsCancellationRequested)
			{
				await ProcessScheduledMessagesAsync();
				await WaitAsync();
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

		private async Task WaitAsync()
		{
			try
			{
				await Task.Delay(pollingInterval, serviceCancellationToken);
			}
			catch (TaskCanceledException)
			{
				// Just swallow it.
			}
		}
	}
}
