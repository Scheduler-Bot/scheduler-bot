using System;
using System.Collections.Generic;
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
using Microsoft.Rest;
using SchedulerBot.Database.Core;
using SchedulerBot.Database.Entities;
using SchedulerBot.Infrastructure.Interfaces;

namespace SchedulerBot.Business.Services
{
	public sealed class ScheduledMessageProcessorService : IHostedService, IDisposable
	{
		private readonly ServiceClientCredentials credentials;
		private readonly IServiceScopeFactory scopeFactory;
		private readonly IScheduleParser scheduleParser;
		private readonly TimeSpan pollingInterval;
		private readonly CancellationTokenSource serviceCancellationTokenSource;
		private readonly CancellationToken serviceCancellationToken;

		public ScheduledMessageProcessorService(ServiceClientCredentials credentials, IServiceScopeFactory scopeFactory, IConfiguration configuration, IScheduleParser scheduleParser)
		{
			this.credentials = credentials;
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
					.Include(message => message.Events)
					.Include(message => message.Details)
					.Where(message => ShouldSendMessage(message, currentTime));

				foreach (ScheduledMessage scheduledMessage in messagesToProcess)
				{
					await SendMessageAsync(scheduledMessage);
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
			ICollection<ScheduledMessageEvent> events = scheduledMessage.Events;
			DateTime lastOccurence = events?.Max(@event => @event.CreatedOn) ?? DateTime.MinValue;
			ISchedule messageSchedule = scheduleParser.Parse(scheduledMessage.Schedule, lastOccurence);
			DateTime nextOccurence = messageSchedule.NextOccurence;

			return nextOccurence >= lastOccurence && nextOccurence <= currentTime;
		}

		private async Task SendMessageAsync(ScheduledMessage scheduledMessage)
		{
			Uri serviceUri = new Uri(scheduledMessage.Details.ServiceUrl);
			Activity activity = CreateMessageActivity(scheduledMessage);

			using (ConnectorClient connector = new ConnectorClient(serviceUri, credentials))
			{
				await connector.Conversations.SendToConversationAsync(activity);
			}

			AddScheduledMessageEvent(scheduledMessage);
		}

		private static Activity CreateMessageActivity(ScheduledMessage scheduledMessage)
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

		private static void AddScheduledMessageEvent(ScheduledMessage scheduledMessage)
		{
			if (scheduledMessage.Events == null)
			{
				scheduledMessage.Events = new List<ScheduledMessageEvent>();
			}

			scheduledMessage.Events.Add(new ScheduledMessageEvent
			{
				CreatedOn = DateTime.UtcNow
			});
		}
	}
}
