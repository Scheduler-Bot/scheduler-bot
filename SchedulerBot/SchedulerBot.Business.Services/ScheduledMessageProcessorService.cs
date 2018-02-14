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
				SchedulerBotContext context = scope.ServiceProvider.GetRequiredService<SchedulerBotContext>();

				foreach (ScheduledMessageEvent scheduledMessageEvent in GetPendingEvents(context))
				{
					ScheduledMessage scheduledMessage = scheduledMessageEvent.ScheduledMessage;

					await SendMessageAsync(scheduledMessage);

					scheduledMessageEvent.State = ScheduledMessageEventState.Completed;
					AddPendingEvent(scheduledMessage);
				}

				await context.SaveChangesAsync(serviceCancellationToken);
			}
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
			Activity activity = CreateMessageActivity(scheduledMessage);

			using (ConnectorClient connector = new ConnectorClient(serviceUri, credentials))
			{
				await connector.Conversations.SendToConversationAsync(activity, serviceCancellationToken);
			}
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
		
		private void AddPendingEvent(ScheduledMessage scheduledMessage)
		{
			DateTime currentTime = DateTime.UtcNow;
			ISchedule schedule = scheduleParser.Parse(scheduledMessage.Schedule, currentTime);

			scheduledMessage.Events.Add(new ScheduledMessageEvent
			{
				CreatedOn = currentTime,
				NextOccurence = schedule.NextOccurence,
				State = ScheduledMessageEventState.Pending
			});
		}
	}
}
