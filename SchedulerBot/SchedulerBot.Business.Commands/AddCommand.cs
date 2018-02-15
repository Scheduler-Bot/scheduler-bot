using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
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

		public AddCommand(SchedulerBotContext context, IScheduleParser scheduleParser, IScheduleDescriptionFormatter scheduleDescriptionFormatter)
		{
			this.context = context;
			this.scheduleParser = scheduleParser;
			this.scheduleDescriptionFormatter = scheduleDescriptionFormatter;

			Name = "add";
		}

		public string Name { get; }

		public async Task<string> ExecuteAsync(Activity activity, string arguments)
		{
			string result;
			string[] splitArguments = ArgumentHelper.ParseArguments(arguments);
			string text = splitArguments.ElementAtOrDefault(0);
			string textSchedule = splitArguments.ElementAtOrDefault(1);

			if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(textSchedule))
			{
				if (scheduleParser.TryParse(textSchedule, DateTime.UtcNow, out ISchedule schedule))
				{
					ScheduledMessage scheduledMessage = CreateScheduledMessageAsync(activity, text, schedule);
					ScheduledMessage createdMessage = (await context.ScheduledMessages.AddAsync(scheduledMessage)).Entity;

					await context.SaveChangesAsync();

					string scheduleDescription = scheduleDescriptionFormatter.Format(schedule, activity.Locale);

					result = $"New event has been created:{Environment.NewLine}ID: '{createdMessage.Id}'{Environment.NewLine}Schedule: {scheduleDescription}";
				}
				else
				{
					result = $"Cannot recognize schedule \"{textSchedule}\"";
				}
			}
			else
			{
				result = "Command arguments are in incorrect format. Use the following pattern: add 'your text' 'your schedule'";
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
