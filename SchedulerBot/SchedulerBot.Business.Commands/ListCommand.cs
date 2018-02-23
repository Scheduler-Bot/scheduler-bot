using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using SchedulerBot.Business.Interfaces;
using SchedulerBot.Business.Interfaces.Entities;
using SchedulerBot.Database.Core;
using SchedulerBot.Database.Entities;
using SchedulerBot.Database.Entities.Enums;

namespace SchedulerBot.Business.Commands
{
	public class ListCommand : IBotCommand
	{
		private readonly SchedulerBotContext context;
		private readonly IMessageListFormatter messageListFormatter;
		private readonly ILogger<ListCommand> logger;

		public ListCommand(
			SchedulerBotContext context,
			IMessageListFormatter messageListFormatter,
			ILogger<ListCommand> logger)
		{
			this.context = context;
			this.messageListFormatter = messageListFormatter;
			this.logger = logger;

			Name = "list";
		}

		public string Name { get; }

		public Task<CommandExecutionResult> ExecuteAsync(Activity activity, string arguments)
		{
			logger.LogInformation("Executing '{0}' command", Name);
			logger.LogInformation("Searching for the scheduled messages for the conversation with id '{0}'", activity.Conversation.Id);

			IQueryable<ScheduledMessage> conversationMessages = GetConversationMessages(activity.Conversation.Id);
			string messages = messageListFormatter.Format(conversationMessages, activity);
			CommandExecutionResult result = CommandExecutionResult.Success(messages);

			return Task.FromResult(result);
		}

		private IQueryable<ScheduledMessage> GetConversationMessages(string conversationId)
		{
			return context.ScheduledMessageDetails
				.Where(details => details.ConversationId.Equals(conversationId, StringComparison.Ordinal)
					&& details.ScheduledMessage.State == ScheduledMessageState.Active)
				.Select(details => details.ScheduledMessage);
		}
	}
}
