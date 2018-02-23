using System;
using NCrontab;
using SchedulerBot.Infrastructure.Interfaces;

namespace SchedulerBot.Infrastructure.Utils
{
	public class CronSchedule : ISchedule
	{
		private readonly CrontabSchedule crontabSchedule;

		public CronSchedule(CrontabSchedule crontabSchedule, string text, TimeSpan? timeZoneOffset)
		{
			this.crontabSchedule = crontabSchedule;
			Text = text;
			TimeZoneOffset = timeZoneOffset;
		}

		public string Text { get; }

		public TimeSpan? TimeZoneOffset { get; }

		public DateTime GetNextOccurence() => GetNextOccurence(DateTime.UtcNow);

		public DateTime GetNextOccurence(DateTime baseTime)
		{
			// base time should be converted to channel timeZoneOffset to prevent possibly issues with +N timezones
			baseTime = TimeZoneOffset.HasValue ? baseTime.Add(TimeZoneOffset.Value) : baseTime;

			DateTime nextOccurence = crontabSchedule.GetNextOccurrence(baseTime);

			// if channel timeZoneOffset provided use it during calculation of nextOccurence in Utc
			nextOccurence = TimeZoneOffset.HasValue ? nextOccurence.Add(-TimeZoneOffset.Value) : nextOccurence;

			return nextOccurence;
		}
	}
}
