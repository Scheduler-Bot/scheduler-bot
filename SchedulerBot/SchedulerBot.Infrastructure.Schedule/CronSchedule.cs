using System;
using System.Collections.Generic;
using System.Linq;
using NCrontab;
using SchedulerBot.Infrastructure.Interfaces.Schedule;

namespace SchedulerBot.Infrastructure.Schedule
{
	/// <summary>
	/// An <see cref="ISchedule"/> implementation for a cron expression format case.
	/// </summary>
	/// <seealso cref="ISchedule" />
	public class CronSchedule : ISchedule
	{
		private readonly CrontabSchedule crontabSchedule;
		private readonly TimeSpan? timeZoneOffset;

		/// <summary>
		/// Initializes a new instance of the <see cref="CronSchedule"/> class.
		/// </summary>
		/// <param name="crontabSchedule">The crontab schedule.</param>
		/// <param name="text">The schedule in cron format.</param>
		/// <param name="timeZoneOffset">The time zone offset. If not provided, no offset will be used for occurrence calculation.</param>
		public CronSchedule(CrontabSchedule crontabSchedule, string text, TimeSpan? timeZoneOffset)
		{
			this.crontabSchedule = crontabSchedule;
			Text = text;
			this.timeZoneOffset = timeZoneOffset;
		}

		/// <summary>
		/// Gets the text schedule representation in a form of cron expression.
		/// </summary>
		public string Text { get; }

		/// <inheritdoc />
		public DateTime GetNextOccurrence() => GetNextOccurrence(DateTime.UtcNow);

		/// <inheritdoc />
		public DateTime GetNextOccurrence(DateTime baseTime)
		{
			DateTime adjustedBaseTime = AdjustDateTime(baseTime);
			DateTime nextOccurrence = crontabSchedule.GetNextOccurrence(adjustedBaseTime);
			DateTime adjustedNextOccurrence = AdjustOccurrence(nextOccurrence);
			
			return adjustedNextOccurrence;
		}

		/// <inheritdoc />
		public IEnumerable<DateTime> GetNextOccurrences(DateTime baseTime, DateTime endTime)
		{
			DateTime adjustedBaseTime = AdjustDateTime(baseTime);
			DateTime adjustedEndTime = AdjustDateTime(endTime);

			return crontabSchedule
				.GetNextOccurrences(adjustedBaseTime, adjustedEndTime)
				.Select(AdjustOccurrence);
		}

		private DateTime AdjustDateTime(DateTime baseTime)
		{
			// base time should be converted to channel timeZoneOffset to prevent possibly issues with +N timezones
			return timeZoneOffset.HasValue && baseTime != DateTime.MaxValue ? baseTime.Add(timeZoneOffset.Value) : baseTime;
		}

		private DateTime AdjustOccurrence(DateTime occurrence)
		{
			// if channel timeZoneOffset provided use it during calculation of nextOccurrence in Utc
			return timeZoneOffset.HasValue && occurrence != DateTime.MinValue ? occurrence.Add(-timeZoneOffset.Value) : occurrence;
		}
	}
}
