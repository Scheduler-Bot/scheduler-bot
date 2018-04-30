using System;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using SchedulerBot.Business.Entities;
using SchedulerBot.Database.Core;
using SchedulerBot.Database.Entities;

namespace SchedulerBot.Business.Commands
{
	/// <summary>
	/// The command allowing for a user to get a link to a temporary page
	/// where they can manage the events for the current conversation.
	/// </summary>
	/// <seealso cref="BotCommand" />
	public class ManageCommand : BotCommand
	{
		private readonly SchedulerBotContext context;

		/// <summary>
		/// Initializes a new instance of the <see cref="ManageCommand"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="logger">The logger.</param>
		public ManageCommand(SchedulerBotContext context, ILogger<ManageCommand> logger) : base("manage", logger)
		{
			this.context = context;
		}

		/// <inheritdoc />
		protected override async Task<CommandExecutionResult> ExecuteCoreAsync(Activity activity, string arguments)
		{
			Logger.LogInformation(
				"Creating a manage link for channel '{0}' and conversation '{1}'",
				activity.ChannelId,
				activity.Conversation.Id);

			ManageConversationLink manageConversationLink = CreateManageConversationLink(activity);

			await context.ManageConversationLinks.AddAsync(manageConversationLink);
			await context.SaveChangesAsync();

			Logger.LogInformation(
				"Created the manage link '{0}' for channel '{1}' and conversation '{2}'",
				manageConversationLink.Text,
				activity.ChannelId,
				activity.Conversation.Id);

			return CommandExecutionResult.Success(manageConversationLink.Text);
		}

		private static ManageConversationLink CreateManageConversationLink(Activity activity)
		{
			string channelId = activity.ChannelId;
			string conversationId = activity.Conversation.Id;
			string linkText = "random-link-text";
			TimeSpan linkExpirationPeriod = TimeSpan.FromMinutes(20);
			DateTime linkCreationTime = DateTime.UtcNow;
			DateTime linkExpirationTime = linkCreationTime + linkExpirationPeriod;

			return new ManageConversationLink
			{
				ChannelId = channelId,
				ConversationId = conversationId,
				Text = linkText,
				CreatedOn = linkCreationTime,
				ExpiresOn = linkExpirationTime
			};
		}
	}
}
