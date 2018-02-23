using System;
using NCrontab;
using SchedulerBot.Infrastructure.Interfaces;

namespace SchedulerBot.Infrastructure.Utils
{
	public class CronScheduleParser : IScheduleParser
	{
		public ISchedule Parse(string textSchedule, TimeSpan? timeZoneOffset)
		{
			CrontabSchedule cronSchedule = CrontabSchedule.Parse(textSchedule);
			CronSchedule schedule = CreateSchedule(textSchedule, cronSchedule, timeZoneOffset);

			return schedule;
		}

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
