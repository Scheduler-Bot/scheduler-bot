using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using SchedulerBot.Business.Commands.Utils;
using SchedulerBot.Business.Entities;
using SchedulerBot.Business.Utils;
using SchedulerBot.Database.Core;
using SchedulerBot.Database.Entities;
using SchedulerBot.Database.Entities.Enums;
using SchedulerBot.Infrastructure.Interfaces.Schedule;

namespace SchedulerBot.Business.Commands
{
	/// <summary>
	/// The command for entering a new scheduled message.
	/// </summary>
	/// <example>Expected input: add 'Turn off the iron!' '0 * * * *'</example>
	/// <seealso cref="BotCommand" />
	public class AddCommand : BotCommand
	{
		#region Private Fields

		private readonly SchedulerBotContext context;
		private readonly IScheduleParser scheduleParser;
		private readonly IScheduleDescriptionFormatter scheduleDescriptionFormatter;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="AddCommand"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="scheduleParser">The schedule parser.</param>
		/// <param name="scheduleDescriptionFormatter">The schedule description formatter.</param>
		/// <param name="logger">The logger.</param>
		public AddCommand(
			SchedulerBotContext context,
			IScheduleParser scheduleParser,
			IScheduleDescriptionFormatter scheduleDescriptionFormatter,
			ILogger<AddCommand> logger) : base("add", logger)
		{
			this.context = context;
			this.scheduleParser = scheduleParser;
			this.scheduleDescriptionFormatter = scheduleDescriptionFormatter;
		}

		#endregion

		#region Overrides

		/// <inheritdoc />
		protected override async Task<CommandExecutionResult> ExecuteCoreAsync(Activity activity, string arguments)
		{
			CommandExecutionResult result;
			string[] splitArguments = ArgumentHelper.ParseArguments(arguments);
			string text = splitArguments.ElementAtOrDefault(0);
			string textSchedule = splitArguments.ElementAtOrDefault(1);

			if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(textSchedule) && splitArguments.Length == 2)
			{
				Logger.LogInformation("Parsed the arguments to text '{0}' and schedule '{1}'", text, textSchedule);

				if (scheduleParser.TryParse(textSchedule, activity.LocalTimestamp?.Offset, out ISchedule schedule))
				{
					Logger.LogInformation("Creating a new scheduled message");

					ScheduledMessage scheduledMessage = CreateScheduledMessageAsync(activity, text, schedule);
					scheduledMessage = (await context.ScheduledMessages.AddAsync(scheduledMessage)).Entity;

					await context.SaveChangesAsync();

					string scheduleDescription = scheduleDescriptionFormatter.Format(schedule, activity.Locale);
					string createdMessageId = scheduledMessage.Id.ToString();
					string newLine = MessageUtils.NewLine;

					result = $"New event has been created:{newLine}ID: '{createdMessageId}'{newLine}Schedule: {scheduleDescription}";
					Logger.LogInformation("Created a scheduled message with id '{0}'", createdMessageId);
				}
				else
				{
					result = CommandExecutionResult.Error($"Cannot recognize schedule \"{textSchedule}\"");
					Logger.LogWarning(result.Message);
				}
			}
			else
			{
				result = CommandExecutionResult.Error("Command arguments are in incorrect format. Use the following pattern: add 'your text' 'your schedule'");
				Logger.LogWarning("Cannot parse the command arguments");
			}

			return result;
		}

		#endregion

		#region Private Methods

		private static ScheduledMessage CreateScheduledMessageAsync(Activity activity, string text, ISchedule schedule)
		{
			return new ScheduledMessage
			{
				Text = text,
				Schedule = schedule.Text,
				State = ScheduledMessageState.Active,
				Details = CreateMessageDetails(activity),
				Events = new List<ScheduledMessageEvent>
				{
					CreateMessageEvent(schedule)
				}
			};
		}

		private static ScheduledMessageDetails CreateMessageDetails(Activity activity)
		{
			return new ScheduledMessageDetails
			{
				ServiceUrl = activity.ServiceUrl,
				FromId = activity.Recipient.Id,
				FromName = activity.Recipient.Name,
				RecipientId = activity.From.Id,
				RecipientName = activity.From.Name,
				ChannelId = activity.ChannelId,
				ConversationId = activity.Conversation.Id,
				Locale = activity.Locale,
				TimeZoneOffset = activity.LocalTimestamp?.Offset
			};
		}

		private static ScheduledMessageEvent CreateMessageEvent(ISchedule schedule)
		{
			return new ScheduledMessageEvent
			{
				CreatedOn = DateTime.UtcNow,
				NextOccurence = schedule.GetNextOccurence(),
				State = ScheduledMessageEventState.Pending
			};
		}

		#endregion
	}
}
