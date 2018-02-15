using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using SchedulerBot.Business.Interfaces;
using SchedulerBot.Database.Core;
using SchedulerBot.Database.Entities;
using SchedulerBot.Database.Entities.Enums;
using SchedulerBot.Infrastructure.Interfaces;

namespace SchedulerBot.Business.Commands
{
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
		}

		public string Name { get; } = "add";

		public async Task<CommandResult> ExecuteAsync(Activity activity, string arguments)
		{
			CommandResult result;
			string textSchedule = arguments;

			if (scheduleParser.TryParse(textSchedule, DateTime.UtcNow, out ISchedule schedule))
			{
				ScheduledMessage scheduledMessage = CreateScheduledMessageAsync(activity, schedule);

				await context.ScheduledMessages.AddAsync(scheduledMessage);
				await context.SaveChangesAsync();

				string scheduleDescription = scheduleDescriptionFormatter.Format(schedule, activity.Locale);

				result = new CommandResult($"Created an event with the following schedule: {scheduleDescription}", succeeded: true);
			}
			else
			{
				result = new CommandResult($"Cannot recognize schedule \"{textSchedule}\"", succeeded: false);
			}

			return result;
		}

		private static ScheduledMessage CreateScheduledMessageAsync(Activity activity, ISchedule schedule)
		{
			return new ScheduledMessage
			{
				Text = "Hello!",
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
