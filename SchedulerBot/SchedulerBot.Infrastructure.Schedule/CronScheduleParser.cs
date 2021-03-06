﻿using System;
using NCrontab;
using SchedulerBot.Infrastructure.Interfaces.Schedule;

namespace SchedulerBot.Infrastructure.Schedule
{
	/// <summary>
	/// An <see cref="IScheduleParser"/> implementation allowing to parse cron expressions.
	/// </summary>
	/// <seealso cref="IScheduleParser" />
	public class CronScheduleParser : IScheduleParser
	{
		/// <inheritdoc />
		public ISchedule Parse(string textSchedule, TimeSpan? timeZoneOffset)
		{
			CrontabSchedule cronSchedule = CrontabSchedule.Parse(textSchedule);
			CronSchedule schedule = CreateSchedule(textSchedule, cronSchedule, timeZoneOffset);

			return schedule;
		}

		/// <inheritdoc />
		public bool TryParse(string textSchedule, TimeSpan? timeZoneOffset, out ISchedule schedule)
		{
			CrontabSchedule cronSchedule = CrontabSchedule.TryParse(textSchedule);
			schedule = cronSchedule != null ? CreateSchedule(textSchedule, cronSchedule, timeZoneOffset) : null;

			return schedule != null;
		}

		private static CronSchedule CreateSchedule(string textSchedule, CrontabSchedule cronSchedule, TimeSpan? timeZoneOffset)
		{
			return new CronSchedule(cronSchedule, textSchedule, timeZoneOffset);
		}
	}
}
