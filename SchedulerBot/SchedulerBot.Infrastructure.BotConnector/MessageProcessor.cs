using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using SchedulerBot.Business.Entities;
using SchedulerBot.Database.Entities;
using SchedulerBot.Infrastructure.Interfaces.BotConnector;

namespace SchedulerBot.Infrastructure.BotConnector
{
	/// <summary>
	/// Implements a way of processing messages.
	/// </summary>
	/// <seealso cref="IMessageProcessor" />
	public class MessageProcessor : IMessageProcessor
	{
		private readonly AppCredentials appCredentials;
		private readonly ILogger<MessageProcessor> logger;

		/// <summary>
		/// Initializes a new instance of the <see cref="MessageProcessor"/> class.
		/// </summary>
		/// <param name="appCredentials">The application credentials.</param>
		/// <param name="logger">The logger.</param>
		public MessageProcessor(
			AppCredentials appCredentials,
			ILogger<MessageProcessor> logger)
		{
			this.appCredentials = appCredentials;
			this.logger = logger;
		}

		/// <inheritdoc />
		public async Task SendMessageAsync(ScheduledMessage scheduledMessage, CancellationToken cancellationToken)
		{
			if (!cancellationToken.IsCancellationRequested)
			{
				MicrosoftAppCredentials credentials = BuildMicrosoftAppCredentials();

				Uri serviceUri = new Uri(scheduledMessage.Details.ServiceUrl);
				Activity activity = CreateBotMessageActivity(scheduledMessage);

				using (ConnectorClient connector = new ConnectorClient(serviceUri, credentials))
				{
					logger.LogInformation($"Sending message to conversation RecipientId: ${activity.Recipient.Id}.");
					await connector.Conversations.SendToConversationAsync(activity, cancellationToken);
				}
			}
			else
			{
				logger.LogInformation("CancellationToken is in CancellationRequested state.");
			}
		}

		/// <inheritdoc />
		public Task<ResourceResponse> ReplyAsync(Activity activity, string replyText, CancellationToken cancellationToken)
		{
			if (!cancellationToken.IsCancellationRequested)
			{
				MicrosoftAppCredentials credentials = BuildMicrosoftAppCredentials();

				Activity reply = activity.CreateReply(replyText, activity.Locale);

				using (ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl), credentials))
				{
					return connector.Conversations.ReplyToActivityAsync(reply, cancellationToken);
				}
			}

			logger.LogInformation("CancellationToken is in CancellationRequested state.");
			return null;
		}

		private MicrosoftAppCredentials BuildMicrosoftAppCredentials()
		{
			MicrosoftAppCredentials credentials = new MicrosoftAppCredentials(
				appCredentials.AppId,
				appCredentials.AppPassword);
			return credentials;
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
