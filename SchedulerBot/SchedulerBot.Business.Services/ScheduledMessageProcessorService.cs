using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SchedulerBot.Database.Core;
using SchedulerBot.Database.Entities;
using SchedulerBot.Database.Entities.Enums;
using SchedulerBot.Infrastructure.Interfaces.BotConnector;
using SchedulerBot.Infrastructure.Interfaces.Schedule;

namespace SchedulerBot.Business.Services
{
	/// <summary>
	/// A service supposed to send out the scheduled messages to their recipients.
	/// </summary>
	/// <seealso cref="IHostedService" />
	/// <seealso cref="IDisposable" />
	public sealed class ScheduledMessageProcessorService : IHostedService, IDisposable
	{
		#region Private Fields

		private readonly IServiceScopeFactory scopeFactory;
		private readonly ILogger<ScheduledMessageProcessorService> logger;
		private readonly IMessageProcessor messageProcessor;
		private readonly IScheduleParser scheduleParser;
		private readonly TimeSpan pollingInterval;
		private readonly CancellationTokenSource serviceCancellationTokenSource;
		private readonly CancellationToken serviceCancellationToken;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ScheduledMessageProcessorService"/> class.
		/// </summary>
		/// <param name="scheduleParser">The schedule parser.</param>
		/// <param name="scopeFactory">The scope factory.</param>
		/// <param name="configuration">The configuration.</param>
		/// <param name="logger">The logger.</param>
		/// <param name="messageProcessor">The message processor.</param>
		public ScheduledMessageProcessorService(
			IScheduleParser scheduleParser,
			IServiceScopeFactory scopeFactory,
			IConfiguration configuration,
			ILogger<ScheduledMessageProcessorService> logger,
			IMessageProcessor messageProcessor)
		{
			this.scheduleParser = scheduleParser;
			this.scopeFactory = scopeFactory;
			this.logger = logger;
			this.messageProcessor = messageProcessor;

			pollingInterval = TimeSpan.Parse(configuration["MessageProcessingInterval"], CultureInfo.InvariantCulture);
			serviceCancellationTokenSource = new CancellationTokenSource();
			serviceCancellationToken = serviceCancellationTokenSource.Token;
		}

		#endregion

		#region IHostedService Implementation

		/// <inheritdoc />
		public async Task StartAsync(CancellationToken cancellationToken)
		{
			logger.LogInformation("Starting polling");

			while (!serviceCancellationTokenSource.IsCancellationRequested)
			{
				try
				{
					await ProcessScheduledMessagesAsync();
				}
				catch (Exception exception)
				{
					logger.LogCritical(exception, $"ProcessScheduledMessagesAsync method execution failed due Exception {exception.Message}. StackTrace: {exception.StackTrace}.");
				}
				finally
				{
					await WaitAsync();
				}
			}

			logger.LogInformation("Polling stopped");
		}

		/// <inheritdoc />
		public Task StopAsync(CancellationToken cancellationToken)
		{
			logger.LogInformation("Stopping polling");
			serviceCancellationTokenSource.Cancel();

			// TODO: make this wait for the real cancellation.
			return Task.CompletedTask;
		}

		#endregion

		#region IDisposable Implementation

		/// <inheritdoc />
		public void Dispose()
		{
			serviceCancellationTokenSource?.Dispose();
		}

		#endregion

		#region Private Methods

		private async Task ProcessScheduledMessagesAsync()
		{
			logger.LogInformation("Starting to process scheduled messages queue");

			using (IServiceScope scope = scopeFactory.CreateScope())
			{
				SchedulerBotContext context = scope.ServiceProvider.GetRequiredService<SchedulerBotContext>();

				// TODO: there can be used parallel foreach
				foreach (ScheduledMessageEvent scheduledMessageEvent in GetPendingEvents(context))
				{
					try
					{
						ScheduledMessage scheduledMessage = scheduledMessageEvent.ScheduledMessage;
						string scheduledMessageId = scheduledMessage.Id.ToString();

						logger.LogInformation("Processing scheduled message '{0}'", scheduledMessageId);

						await messageProcessor.SendMessageAsync(scheduledMessage, serviceCancellationToken);

						scheduledMessageEvent.State = ScheduledMessageEventState.Completed;
						AddPendingEvent(scheduledMessage);
						logger.LogInformation("Scheduled message '{0}' has been processed", scheduledMessageId);
					}
					catch (Exception exception)
					{
						logger.LogError(exception, $"Sending message was failed due Exception {exception.Message}. StackTrace: {exception.StackTrace}.");
					}
				}

				await context.SaveChangesAsync(serviceCancellationToken);
			}

			logger.LogInformation("Finished processing scheduled messages queue");
		}

		private static IQueryable<ScheduledMessageEvent> GetPendingEvents(SchedulerBotContext context)
		{
			DateTime currentTime = DateTime.UtcNow;

			return context
				.ScheduledMessageEvents
				.Where(@event => @event.State == ScheduledMessageEventState.Pending && @event.NextOccurence < currentTime)
				.Include(@event => @event.ScheduledMessage)
				.ThenInclude(message => message.Details);
		}

		private async Task WaitAsync()
		{
			try
			{
				logger.LogInformation("Waiting for the next poll");

				await Task.Delay(pollingInterval, serviceCancellationToken);
			}
			catch (TaskCanceledException)
			{
				// Just swallow it.
			}
		}

		private void AddPendingEvent(ScheduledMessage scheduledMessage)
		{
			ISchedule schedule = scheduleParser.Parse(scheduledMessage.Schedule, scheduledMessage.Details.TimeZoneOffset);

			scheduledMessage.Events.Add(new ScheduledMessageEvent
			{
				CreatedOn = DateTime.UtcNow,
				NextOccurence = schedule.GetNextOccurence(),
				State = ScheduledMessageEventState.Pending
			});
		}

		#endregion
	}
}
