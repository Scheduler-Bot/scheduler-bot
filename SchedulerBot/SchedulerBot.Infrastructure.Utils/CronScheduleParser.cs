using System;
using NCrontab;
using SchedulerBot.Infrastructure.Interfaces;

namespace SchedulerBot.Infrastructure.Utils
{
	public class CronScheduleParser : IScheduleParser
	{
		public ISchedule Parse(string textSchedule, DateTime baseTime)
		{
			CrontabSchedule cronSchedule = CrontabSchedule.Parse(textSchedule);
			Schedule schedule = CreateSchedule(textSchedule, baseTime, cronSchedule);

			return schedule;
		}

		public bool TryParse(string textSchedule, DateTime baseTime, out ISchedule schedule)
		{
			CrontabSchedule cronSchedule = CrontabSchedule.TryParse(textSchedule);
			schedule = cronSchedule != null ? CreateSchedule(textSchedule, baseTime, cronSchedule) : null;

			return schedule != null;
		}

		private static Schedule CreateSchedule(string textSchedule, DateTime baseTime, CrontabSchedule cronSchedule)
		{
			DateTime nextOccurence = cronSchedule.GetNextOccurrence(baseTime);
			Schedule schedule = new Schedule(textSchedule, nextOccurence);

			return schedule;
		}
	}
}
