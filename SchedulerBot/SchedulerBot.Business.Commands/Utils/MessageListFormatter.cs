using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using SchedulerBot.Business.Interfaces;
using SchedulerBot.Business.Utils;
using SchedulerBot.Database.Entities;
using SchedulerBot.Infrastructure.Interfaces;

namespace SchedulerBot.Business.Commands.Utils
{
	public class MessageListFormatter : IMessageListFormatter
	{
		private readonly IScheduleParser scheduleParser;
		private readonly IScheduleDescriptionFormatter scheduleDescriptionFormatter;
		private readonly ILogger<MessageListFormatter> logger;

		public MessageListFormatter(
			IScheduleParser scheduleParser,
			IScheduleDescriptionFormatter scheduleDescriptionFormatter,
			ILogger<MessageListFormatter> logger)
		{
			this.scheduleParser = scheduleParser;
			this.scheduleDescriptionFormatter = scheduleDescriptionFormatter;
			this.logger = logger;
		}

		public string Format(IEnumerable<ScheduledMessage> scheduledMessages, Activity activity)
		{
			int messageCount = 0;
			StringBuilder stringBuilder = new StringBuilder();

			foreach (ScheduledMessage scheduledMessage in scheduledMessages)
			{
				AppendMessageDescription(scheduledMessage, stringBuilder, activity.Locale, activity.LocalTimestamp?.Offset);
				messageCount++;
			}

			string result;

			if (messageCount > 0)
			{
				result = stringBuilder.ToString().Trim();
				logger.LogInformation("Found '{0}' scheduled messages for the conversation '{1}'", messageCount, activity.Conversation.Id);
			}
			else
			{
				result = "No scheduled events for this conversation";
				logger.LogInformation("No schedule messages found for the conversation '{1}'", activity.Conversation.Id);
			}

			return result;
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
