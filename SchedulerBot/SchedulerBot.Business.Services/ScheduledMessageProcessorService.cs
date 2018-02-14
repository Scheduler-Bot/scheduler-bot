using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SchedulerBot.Database.Core;
using SchedulerBot.Database.Entities;
using SchedulerBot.Infrastructure.Interfaces;

namespace SchedulerBot.Business.Services
{
	public sealed class ScheduledMessageProcessorService : IHostedService, IDisposable
	{
		private readonly IServiceScopeFactory scopeFactory;
		private readonly IScheduleParser scheduleParser;
		private readonly TimeSpan pollingInterval;
		private readonly CancellationTokenSource serviceCancellationTokenSource;
		private readonly CancellationToken serviceCancellationToken;

		public ScheduledMessageProcessorService(IServiceScopeFactory scopeFactory, IConfiguration configuration, IScheduleParser scheduleParser)
		{
			this.scopeFactory = scopeFactory;
			this.scheduleParser = scheduleParser;
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
				DateTime currentTime = DateTime.UtcNow;
				SchedulerBotContext context = scope.ServiceProvider.GetRequiredService<SchedulerBotContext>();
				IQueryable<ScheduledMessage> messagesToProcess = context
					.ScheduledMessages
					.Include(message => message.Logs)
					.Where(message => ShouldSendMessage(message, currentTime));

				foreach (ScheduledMessage scheduledMessage in messagesToProcess)
				{
					// TODO: send message
				}

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

		private bool ShouldSendMessage(ScheduledMessage scheduledMessage, DateTime currentTime)
		{
			ICollection<ScheduledMessageLog> logs = scheduledMessage.Logs;
			DateTime lastOccurence = logs?.Max(log => log.CreatedOn) ?? DateTime.MinValue;
			ISchedule messageSchedule = scheduleParser.Parse(scheduledMessage.Schedule, lastOccurence);
			DateTime nextOccurence = messageSchedule.NextOccurence;

			return nextOccurence >= lastOccurence && nextOccurence <= currentTime;
		}
	}
}
