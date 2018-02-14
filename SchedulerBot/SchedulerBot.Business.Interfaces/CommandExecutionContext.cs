using Microsoft.Bot.Schema;
using SchedulerBot.Database.Core;
using SchedulerBot.Infrastructure.Interfaces;

namespace SchedulerBot.Business.Interfaces
{
	public class CommandExecutionContext
	{
		public CommandExecutionContext(
			SchedulerBotContext dbContext,
			Activity activity,
			IScheduleParser scheduleParser,
			IScheduleDescriptionFormatter scheduleDescriptionFormatter)
		{
			DbContext = dbContext;
			Activity = activity;
			ScheduleParser = scheduleParser;
			ScheduleDescriptionFormatter = scheduleDescriptionFormatter;
		}

		public SchedulerBotContext DbContext { get; }

		public Activity Activity { get; }

		public IScheduleParser ScheduleParser { get; }

		public IScheduleDescriptionFormatter ScheduleDescriptionFormatter { get; }
	}
}
