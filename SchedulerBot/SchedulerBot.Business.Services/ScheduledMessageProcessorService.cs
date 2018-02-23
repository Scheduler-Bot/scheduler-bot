using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Rest;
using SchedulerBot.Database.Core;
using SchedulerBot.Database.Entities;
using SchedulerBot.Database.Entities.Enums;
using SchedulerBot.Infrastructure.Interfaces;

namespace SchedulerBot.Business.Services
{
	public sealed class ScheduledMessageProcessorService : IHostedService, IDisposable
	{
		private readonly ServiceClientCredentials credentials;
		private readonly IServiceScopeFactory scopeFactory;
		private readonly ILogger<ScheduledMessageProcessorService> logger;
		private readonly IScheduleParser scheduleParser;
		private readonly TimeSpan pollingInterval;
		private readonly CancellationTokenSource serviceCancellationTokenSource;
		private readonly CancellationToken serviceCancellationToken;

		public ScheduledMessageProcessorService(
			ServiceClientCredentials credentials,
			IScheduleParser scheduleParser,
			IServiceScopeFactory scopeFactory,
			IConfiguration configuration,
			ILogger<ScheduledMessageProcessorService> logger)
		{
			this.credentials = credentials;
			this.scheduleParser = scheduleParser;
			this.scopeFactory = scopeFactory;
			this.logger = logger;

			pollingInterval = TimeSpan.Parse(configuration["MessageProcessingInterval"], CultureInfo.InvariantCulture);
			serviceCancellationTokenSource = new CancellationTokenSource();
			serviceCancellationToken = serviceCancellationTokenSource.Token;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			logger.LogInformation("Starting polling");

			while (!serviceCancellationTokenSource.IsCancellationRequested)
			{
				await ProcessScheduledMessagesAsync();
				await WaitAsync();
			}

			logger.LogInformation("Polling stopped");
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			logger.LogInformation("Stopping polling");
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
			logger.LogInformation("Starting to process scheduled messages queue");

			using (IServiceScope scope = scopeFactory.CreateScope())
			{
				SchedulerBotContext context = scope.ServiceProvider.GetRequiredService<SchedulerBotContext>();

				foreach (ScheduledMessageEvent scheduledMessageEvent in GetPendingEvents(context))
				{
					ScheduledMessage scheduledMessage = scheduledMessageEvent.ScheduledMessage;
					string scheduledMessageId = scheduledMessage.Id.ToString();

					logger.LogInformation("Processing scheduled message '{0}'", scheduledMessageId);

					await SendMessageAsync(scheduledMessage);

					scheduledMessageEvent.State = ScheduledMessageEventState.Completed;
					AddPendingEvent(scheduledMessage);
					logger.LogInformation("Scheduled message '{0}' has been processed", scheduledMessageId);
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

		private async Task SendMessageAsync(ScheduledMessage scheduledMessage)
		{
			Uri serviceUri = new Uri(scheduledMessage.Details.ServiceUrl);
			Activity activity = CreateBotMessageActivity(scheduledMessage);

			using (ConnectorClient connector = new ConnectorClient(serviceUri, credentials))
			{
				await connector.Conversations.SendToConversationAsync(activity, serviceCancellationToken);
			}
		}

		private static Activity CreateBotMessageActivity(ScheduledMessage scheduledMessage)
		{
			ScheduledMessageDetails details = scheduledMessage.Details;
			Activity activity = (Activity)Activity.CreateMessageActivity();

			activity.ServiceUrl = details.ServiceUrl;
			activity.From = new ChannelAccount(details.FromId, details.FromName);
			activity.Recipient = new ChannelAccount(details.RecipientId, details.RecipientName);
			activity.ChannelId = details.ChannelId;
			activity.Conversation = new ConversationAccount(id: details.ConversationId);
			activity.Locale = details.Locale;
			activity.Text = scheduledMessage.Text;

			return activity;
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
	}
}
