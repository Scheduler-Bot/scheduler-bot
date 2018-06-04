using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using SchedulerBot.Business.Entities;
using SchedulerBot.Business.Utils;
using SchedulerBot.Database.Core;
using SchedulerBot.Database.Entities;
using SchedulerBot.Database.Entities.Enums;
using SchedulerBot.Database.Interfaces;
using SchedulerBot.Infrastructure.Interfaces.Schedule;

namespace SchedulerBot.Business.Commands
{
	/// <summary>
	/// The command allowing to list the scheduled messages for the current conversation.
	/// </summary>
	/// <seealso cref="BotCommand" />
	public class ListCommand : BotCommand
	{
		#region Private Fields

		private readonly SchedulerBotContext context;
		private readonly IScheduleParser scheduleParser;
		private readonly IScheduleDescriptionFormatter scheduleDescriptionFormatter;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ListCommand" /> class.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="scheduleParser">The schedule parser.</param>
		/// <param name="scheduleDescriptionFormatter">The schedule description formatter.</param>
		/// <param name="unitOfWork">The unit of work.</param>
		/// <param name="logger">The logger.</param>
		public ListCommand(
			SchedulerBotContext context,
			IScheduleParser scheduleParser,
			IScheduleDescriptionFormatter scheduleDescriptionFormatter,
			IUnitOfWork unitOfWork,
			ILogger<ListCommand> logger) : base("list", unitOfWork, logger)
		{
			this.context = context;
			this.scheduleParser = scheduleParser;
			this.scheduleDescriptionFormatter = scheduleDescriptionFormatter;
		}

		#endregion

		#region Overrides

		/// <inheritdoc />
		protected override Task<CommandExecutionResult> ExecuteCoreAsync(Activity activity, string arguments)
		{
			int messageCount = 0;
			string conversationId = activity.Conversation.Id;
			StringBuilder stringBuilder = new StringBuilder();

			Logger.LogInformation("Searching for the scheduled messages for the conversation with id '{0}'", conversationId);

			foreach (ScheduledMessage message in GetConversationMessages(conversationId))
			{
				AppendMessageDescription(message, stringBuilder, activity.Locale, activity.LocalTimestamp?.Offset);
				messageCount++;
			}

			CommandExecutionResult result;

			if (messageCount > 0)
			{
				result = stringBuilder.ToString().Trim();
				Logger.LogInformation("Found '{0}' scheduled messages for the conversation '{1}'", messageCount, conversationId);
			}
			else
			{
				result = CommandExecutionResult.Success("No scheduled events for this conversation");
				Logger.LogInformation("No schedule messages found for the conversation '{1}'", conversationId);
			}

			return Task.FromResult(result);
		}

		#endregion

		#region Private Methods

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

		#endregion
	}
}
