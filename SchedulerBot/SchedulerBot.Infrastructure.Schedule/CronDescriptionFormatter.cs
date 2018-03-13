using CronExpressionDescriptor;
using SchedulerBot.Infrastructure.Interfaces.Schedule;

namespace SchedulerBot.Infrastructure.Schedule
{
	/// <summary>
	/// A <see cref="IScheduleDescriptionFormatter"/> allowing formatting schedules in cron format.
	/// </summary>
	/// <seealso cref="IScheduleDescriptionFormatter" />
	public class CronDescriptionFormatter : IScheduleDescriptionFormatter
	{
		/// <inheritdoc />
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
