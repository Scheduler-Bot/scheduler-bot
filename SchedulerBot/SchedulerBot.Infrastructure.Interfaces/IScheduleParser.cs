using System;

namespace SchedulerBot.Infrastructure.Interfaces
{
	public interface IScheduleParser
	{
		ISchedule Parse(string textSchedule, DateTime baseTime);

		bool TryParse(string textSchedule, DateTime baseTime, out ISchedule schedule);
	}
}
