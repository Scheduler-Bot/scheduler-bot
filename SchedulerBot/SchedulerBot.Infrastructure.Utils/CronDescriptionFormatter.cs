using CronExpressionDescriptor;
using SchedulerBot.Infrastructure.Interfaces;

namespace SchedulerBot.Infrastructure.Utils
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
