using CronExpressionDescriptor;
using SchedulerBot.Infrastructure.Interfaces;
using SchedulerBot.Infrastructure.Interfaces.Schedule;

namespace SchedulerBot.Infrastructure.Scheduler
{
	public class CronDescriptionFormatter : IScheduleDescriptionFormatter
	{
		public string Format(ISchedule schedule, string locale)
		{
			Options options = new Options
			{
				Locale = locale
			};
			string description = ExpressionDescriptor.GetDescription(schedule.Text, options);

			return description;
		}
	}
}
