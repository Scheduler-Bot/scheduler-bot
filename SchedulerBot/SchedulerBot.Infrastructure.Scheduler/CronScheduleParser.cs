﻿using System;
using NCrontab;
using SchedulerBot.Infrastructure.Interfaces.Schedule;

namespace SchedulerBot.Infrastructure.Scheduler
{
	public class CronScheduleParser : IScheduleParser
	{
		public ISchedule Parse(string textSchedule, DateTime baseTime, TimeSpan? timeZoneOffset)
		{
			CrontabSchedule cronSchedule = CrontabSchedule.Parse(textSchedule);
			Schedule schedule = CreateSchedule(textSchedule, cronSchedule, baseTime, timeZoneOffset);

			return schedule;
		}

		public bool TryParse(string textSchedule, DateTime baseTime, TimeSpan? timeZoneOffset, out ISchedule schedule)
		{
			CrontabSchedule cronSchedule = CrontabSchedule.TryParse(textSchedule);
			schedule = cronSchedule != null ? CreateSchedule(textSchedule, cronSchedule, baseTime, timeZoneOffset) : null;

			return schedule != null;
		}

		private static Schedule CreateSchedule(string textSchedule, CrontabSchedule cronSchedule, DateTime baseTime, TimeSpan? timeZoneOffset = null)
		{
			// base time should be converted to channel timeZoneOffset to prevent possibly issues with +N timezones
			baseTime = timeZoneOffset != null ? baseTime.Add(timeZoneOffset.Value) : baseTime;

			DateTime nextOccurence = cronSchedule.GetNextOccurrence(baseTime);
			// if channel timeZoneOffset provided use it during calculation of nextOccurence in Utc
			nextOccurence = timeZoneOffset != null ? nextOccurence.Add(-timeZoneOffset.Value) : nextOccurence;

			Schedule schedule = new Schedule(textSchedule, nextOccurence, timeZoneOffset);

			return schedule;
		}
	}
}
