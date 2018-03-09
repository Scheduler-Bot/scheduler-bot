using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Schema;
using SchedulerBot.Business.Entities;
using SchedulerBot.Database.Entities;
using SchedulerBot.Infrastructure.Interfaces.BotConnector;

namespace SchedulerBot.Infrastructure.BotConnector
{
	public class MessageProcessor : IMessageProcessor
	{
		private readonly AppCredentials appCredentials;

		public MessageProcessor(AppCredentials appCredentials)
		{
			this.appCredentials = appCredentials;
		}

		public async Task SendMessageAsync(ScheduledMessage scheduledMessage, CancellationToken cancellationToken)
		{
			MicrosoftAppCredentials credentials = new MicrosoftAppCredentials(
				appCredentials.AppId,
				appCredentials.AppPassword);

			Uri serviceUri = new Uri(scheduledMessage.Details.ServiceUrl);
			Activity activity = CreateBotMessageActivity(scheduledMessage);

			using (ConnectorClient connector = new ConnectorClient(serviceUri, credentials))
			{
				await connector.Conversations.SendToConversationAsync(activity, cancellationToken);
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
	}
}
