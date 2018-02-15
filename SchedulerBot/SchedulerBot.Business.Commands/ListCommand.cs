using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using Microsoft.EntityFrameworkCore;
using SchedulerBot.Business.Interfaces;
using SchedulerBot.Database.Core;
using SchedulerBot.Database.Entities;
using SchedulerBot.Infrastructure.Interfaces;

namespace SchedulerBot.Business.Commands
{
	public class ListCommand : IBotCommand
	{
		private readonly SchedulerBotContext context;
		private readonly IScheduleParser scheduleParser;
		private readonly IScheduleDescriptionFormatter scheduleDescriptionFormatter;

		public ListCommand(SchedulerBotContext context, IScheduleParser scheduleParser, IScheduleDescriptionFormatter scheduleDescriptionFormatter)
		{
			this.context = context;
			this.scheduleParser = scheduleParser;
			this.scheduleDescriptionFormatter = scheduleDescriptionFormatter;

			Name = "list";
		}

		public string Name { get; }

		public Task<string> ExecuteAsync(Activity activity, string arguments)
		{
			bool messagesFound = false;
			StringBuilder stringBuilder = new StringBuilder();

			foreach (ScheduledMessage message in GetConversationMessages(activity.Conversation.Id))
			{
				messagesFound = true;
				AppendMessageDescription(message, activity.Locale, stringBuilder);
			}

			string result = messagesFound ? stringBuilder.ToString() : "No scheduled events for this conversation";

			return Task.FromResult(result);
		}

		private IQueryable<ScheduledMessage> GetConversationMessages(string conversationId)
		{
			return context.ScheduledMessageDetails
				.Where(details => details.ConversationId.Equals(conversationId, StringComparison.Ordinal))
				.Include(details => details.ScheduledMessage)
				.Select(details => details.ScheduledMessage);
		}

		private void AppendMessageDescription(ScheduledMessage message, string locale, StringBuilder stringBuilder)
		{
			DateTime currentTime = DateTime.UtcNow;
			ISchedule schedule = scheduleParser.Parse(message.Schedule, currentTime);
			string scheduleDescription = scheduleDescriptionFormatter.Format(schedule, locale);

			stringBuilder
				.AppendLine()
				.AppendFormat("ID: {0}", message.Id)
				.AppendLine()
				.AppendFormat("Text: {0}", message.Text)
				.AppendLine()
				.AppendFormat("Schedule: {0}", scheduleDescription)
				.AppendLine();
		}
	}
}
