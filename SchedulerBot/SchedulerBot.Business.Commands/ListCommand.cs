using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using SchedulerBot.Business.Interfaces;
using SchedulerBot.Business.Interfaces.Entities;
using SchedulerBot.Business.Utils;
using SchedulerBot.Database.Core;
using SchedulerBot.Database.Entities;
using SchedulerBot.Database.Entities.Enums;
using SchedulerBot.Infrastructure.Interfaces;

namespace SchedulerBot.Business.Commands
{
	public class ListCommand : IBotCommand
	{
		private readonly SchedulerBotContext context;
		private readonly IScheduleParser scheduleParser;
		private readonly IScheduleDescriptionFormatter scheduleDescriptionFormatter;
		private readonly ILogger<ListCommand> logger;

		public ListCommand(
			SchedulerBotContext context,
			IScheduleParser scheduleParser,
			IScheduleDescriptionFormatter scheduleDescriptionFormatter,
			ILogger<ListCommand> logger)
		{
			this.context = context;
			this.scheduleParser = scheduleParser;
			this.scheduleDescriptionFormatter = scheduleDescriptionFormatter;
			this.logger = logger;

			Name = "list";
		}

		public string Name { get; }

		public Task<CommandExecutionResult> ExecuteAsync(Activity activity, string arguments)
		{
			logger.LogInformation("Executing '{0}' command", Name);

			int messageCount = 0;
			string conversationId = activity.Conversation.Id;
			StringBuilder stringBuilder = new StringBuilder();

			logger.LogInformation("Searching for the scheduled messages for the conversation with id '{0}'", conversationId);

			foreach (ScheduledMessage message in GetConversationMessages(conversationId))
			{
				AppendMessageDescription(message, stringBuilder, activity.Locale, activity.LocalTimestamp?.Offset);
				messageCount++;
			}

			CommandExecutionResult result;

			if (messageCount > 0)
			{
				result = stringBuilder.ToString().Trim();
				logger.LogInformation("Found '{0}' scheduled messages for the conversation '{1}'", messageCount, conversationId);
			}
			else
			{
				result = CommandExecutionResult.Success("No scheduled events for this conversation");
				logger.LogInformation("No schedule messages found for the conversation '{1}'", conversationId);
			}

			return Task.FromResult(result);
		}

		private IQueryable<ScheduledMessage> GetConversationMessages(string conversationId)
		{
			return context.ScheduledMessageDetails
				.Where(details => details.ConversationId.Equals(conversationId, StringComparison.Ordinal)
					&& details.ScheduledMessage.State == ScheduledMessageState.Active)
				.Select(details => details.ScheduledMessage);
		}

		private void AppendMessageDescription(ScheduledMessage message, StringBuilder stringBuilder, string locale, TimeSpan? timeZoneOffset)
		{
			// No need to pass timezone offset in the reason that message is displayed for the user in hist time zone (possibly not UTC)
			ISchedule schedule = scheduleParser.Parse(message.Schedule, timeZoneOffset);
			string scheduleDescription = scheduleDescriptionFormatter.Format(schedule, locale);
			string newLine = MessageUtils.NewLine;
			const string messageSeparator = "----------------------------------";

			stringBuilder
				.AppendFormat("ID: {0}", message.Id)
				.Append(newLine)
				.AppendFormat("Text: {0}", message.Text)
				.Append(newLine)
				.AppendFormat("Schedule: {0}", scheduleDescription)
				.Append(newLine)
				.Append(messageSeparator)
				.Append(newLine);
		}
	}
}
