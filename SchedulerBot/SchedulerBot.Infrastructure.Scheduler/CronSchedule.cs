using System;
using System.Collections.Generic;
using System.Linq;
using NCrontab;
using SchedulerBot.Infrastructure.Interfaces.Schedule;

namespace SchedulerBot.Infrastructure.Scheduler
{
	public class CronSchedule : ISchedule
	{
		private readonly CrontabSchedule crontabSchedule;
		private TimeSpan? timeZoneOffset;

		public CronSchedule(CrontabSchedule crontabSchedule, string text, TimeSpan? timeZoneOffset)
		{
			this.crontabSchedule = crontabSchedule;
			Text = text;
			this.timeZoneOffset = timeZoneOffset;
		}

		public string Text { get; }

		public DateTime GetNextOccurence() => GetNextOccurence(DateTime.UtcNow);

		public DateTime GetNextOccurence(DateTime baseTime)
		{
			DateTime adjustedBaseTime = AdjustDateTime(baseTime);
			DateTime nextOccurence = crontabSchedule.GetNextOccurrence(adjustedBaseTime);
			DateTime adjustedNextOccurence = AdjustOccurence(nextOccurence);
			
			return adjustedNextOccurence;
		}

		public IEnumerable<DateTime> GetNextOccurences(DateTime baseTime, DateTime endTime)
		{
			DateTime adjustedBaseTime = AdjustDateTime(baseTime);
			DateTime adjustedEndTime = AdjustDateTime(endTime);

			return crontabSchedule
				.GetNextOccurrences(adjustedBaseTime, adjustedEndTime)
				.Select(AdjustOccurence);
		}

		private DateTime AdjustDateTime(DateTime baseTime)
		{
			// base time should be converted to channel timeZoneOffset to prevent possibly issues with +N timezones
			return timeZoneOffset.HasValue && baseTime != DateTime.MaxValue ? baseTime.Add(timeZoneOffset.Value) : baseTime;
		}

		private DateTime AdjustOccurence(DateTime occurence)
		{
			// if channel timeZoneOffset provided use it during calculation of nextOccurence in Utc
			return timeZoneOffset.HasValue && occurence != DateTime.MinValue ? occurence.Add(-timeZoneOffset.Value) : occurence;
		}
	}
}
