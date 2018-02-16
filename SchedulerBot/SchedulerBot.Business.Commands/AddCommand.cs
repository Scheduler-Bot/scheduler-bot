using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using SchedulerBot.Business.Commands.Utils;
using SchedulerBot.Business.Interfaces;
using SchedulerBot.Database.Core;
using SchedulerBot.Database.Entities;
using SchedulerBot.Database.Entities.Enums;
using SchedulerBot.Infrastructure.Interfaces;

namespace SchedulerBot.Business.Commands
{
	// Expected input: add 'Turn off the iron!' '0 * * * *'
	public class AddCommand : IBotCommand
	{
		private readonly SchedulerBotContext context;
		private readonly IScheduleParser scheduleParser;
		private readonly IScheduleDescriptionFormatter scheduleDescriptionFormatter;
		private readonly ILogger<AddCommand> logger;

		public AddCommand(
			SchedulerBotContext context,
			IScheduleParser scheduleParser,
			IScheduleDescriptionFormatter scheduleDescriptionFormatter,
			ILogger<AddCommand> logger)
		{
			this.context = context;
			this.scheduleParser = scheduleParser;
			this.scheduleDescriptionFormatter = scheduleDescriptionFormatter;
			this.logger = logger;

			Name = "add";
		}

		public string Name { get; }

		public async Task<string> ExecuteAsync(Activity activity, string arguments)
		{
			logger.LogInformation("Executing '{0}' command with arguments '{1}'", Name, arguments);

			string result;
			string[] splitArguments = ArgumentHelper.ParseArguments(arguments);
			string text = splitArguments.ElementAtOrDefault(0);
			string textSchedule = splitArguments.ElementAtOrDefault(1);

			if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(textSchedule))
			{
				logger.LogInformation("Parsed the arguments to text '{0}' and schedule '{1}'", text, textSchedule);

				if (scheduleParser.TryParse(textSchedule, DateTime.UtcNow, out ISchedule schedule))
				{
					logger.LogInformation("Creating a new scheduled message");

					ScheduledMessage scheduledMessage = CreateScheduledMessageAsync(activity, text, schedule);
					ScheduledMessage createdMessage = (await context.ScheduledMessages.AddAsync(scheduledMessage)).Entity;

					await context.SaveChangesAsync();

					string scheduleDescription = scheduleDescriptionFormatter.Format(schedule, activity.Locale);
					string createdMessageId = createdMessage.Id.ToString();

					result = $"New event has been created:{Environment.NewLine}ID: '{createdMessageId}'{Environment.NewLine}Schedule: {scheduleDescription}";
					logger.LogInformation("Created a scheduled message with id '{0}'", createdMessageId);
				}
				else
				{
					result = $"Cannot recognize schedule \"{textSchedule}\"";
					logger.LogWarning(result);
				}
			}
			else
			{
				result = "Command arguments are in incorrect format. Use the following pattern: add 'your text' 'your schedule'";
				logger.LogWarning("Cannot parse the command arguments");
			}

			return result;
		}

		private static ScheduledMessage CreateScheduledMessageAsync(Activity activity, string text, ISchedule schedule)
		{
			return new ScheduledMessage
			{
				Text = text,
				Schedule = schedule.Text,
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
				Locale = activity.Locale
			};
		}

		private static ScheduledMessageEvent CreateMessageEvent(ISchedule schedule)
		{
			return new ScheduledMessageEvent
			{
				CreatedOn = DateTime.UtcNow,
				NextOccurence = schedule.NextOccurence,
				State = ScheduledMessageEventState.Pending
			};
		}
	}
}
