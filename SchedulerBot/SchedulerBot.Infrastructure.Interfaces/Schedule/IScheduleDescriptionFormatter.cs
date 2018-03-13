namespace SchedulerBot.Infrastructure.Interfaces.Schedule
{
	/// <summary>
	/// Defines an interface for a schedule description formatter.
	/// </summary>
	public interface IScheduleDescriptionFormatter
	{
		/// <summary>
		/// Formats the specified schedule for the specified locale.
		/// </summary>
		/// <param name="schedule">The schedule.</param>
		/// <param name="locale">The locale (e.g. "EN-us").</param>
		/// <returns>The schedule description.</returns>
		string Format(ISchedule schedule, string locale);
	}
}
