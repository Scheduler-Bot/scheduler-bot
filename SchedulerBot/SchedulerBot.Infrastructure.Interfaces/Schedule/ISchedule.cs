using System;
using System.Collections.Generic;

namespace SchedulerBot.Infrastructure.Interfaces.Schedule
{
	/// <summary>
	/// Defines an interface for a schedule.
	/// </summary>
	public interface ISchedule
	{
		/// <summary>
		/// Gets the text schedule representation.
		/// </summary>
		string Text { get; }

		/// <summary>
		/// Gets the next event occurrence.
		/// </summary>
		/// <returns>The time of the next event occurrence.</returns>
		DateTime GetNextOccurence();

		/// <summary>
		/// Gets the next event occurrence relatively to the specified time.
		/// </summary>
		/// <param name="baseTime">The base time.</param>
		/// <returns>The time of the next event occurrence.</returns>
		DateTime GetNextOccurence(DateTime baseTime);

		/// <summary>
		/// Gets the next event occurrences between the specified base and end time.
		/// </summary>
		/// <param name="baseTime">The base time.</param>
		/// <param name="endTime">The end time.</param>
		/// <returns>A set of the next event occurrences.</returns>
		IEnumerable<DateTime> GetNextOccurences(DateTime baseTime, DateTime endTime);
	}
}
