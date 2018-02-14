using System;
using NCrontab;
using SchedulerBot.Infrastructure.Interfaces;

namespace SchedulerBot.Infrastructure.Utils
{
	public class CronScheduleParser : IScheduleParser
	{
		public ISchedule Parse(string textSchedule)
		{
			CrontabSchedule cronSchedule = CrontabSchedule.Parse(textSchedule);
			Schedule schedule = CreateScheduleRelativeToNow(textSchedule, cronSchedule);

			return schedule;
		}

		public bool TryParse(string textSchedule, out ISchedule schedule)
		{
			CrontabSchedule cronSchedule = CrontabSchedule.TryParse(textSchedule);
			schedule = cronSchedule != null ? CreateScheduleRelativeToNow(textSchedule, cronSchedule) : null;

			return schedule != null;
		}

		private static Schedule CreateScheduleRelativeToNow(string textSchedule, CrontabSchedule cronSchedule)
		{
			DateTime nextOccurence = cronSchedule.GetNextOccurrence(DateTime.UtcNow);
			Schedule schedule = new Schedule(textSchedule, nextOccurence);

			return schedule;
		}
	}
}
