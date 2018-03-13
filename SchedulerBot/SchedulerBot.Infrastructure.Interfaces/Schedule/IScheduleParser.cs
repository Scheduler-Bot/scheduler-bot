using System;

namespace SchedulerBot.Infrastructure.Interfaces.Schedule
{
	/// <summary>
	/// Describes a mechanism of parsing a schedule in text form.
	/// </summary>
	public interface IScheduleParser
	{
		/// <summary>
		/// Converts a string representation of a schedule to an <see cref="ISchedule"/> instance.
		/// </summary>
		/// <param name="textSchedule">The text schedule.</param>
		/// <param name="timeZoneOffset">The time zone offset. If not provided, the schedule is constructed without considering an offset.</param>
		/// <returns><see cref="ISchedule"/> instance describing the provided schedule.</returns>
		ISchedule Parse(string textSchedule, TimeSpan? timeZoneOffset);

		/// <summary>
		/// Converts a string representation of a schedule to an <see cref="ISchedule"/> instance.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="textSchedule">The text schedule.</param>
		/// <param name="timeZoneOffset">The time zone offset. If not provided, the schedule is constructed without considering an offset.</param>
		/// <param name="schedule">The <see cref="ISchedule"/> instance if the conversion succeeded, otherwise <c>null</c>.</param>
		/// <returns>A boolean value indicating whether the conversion succeeded.</returns>
		bool TryParse(string textSchedule, TimeSpan? timeZoneOffset, out ISchedule schedule);
	}
}
