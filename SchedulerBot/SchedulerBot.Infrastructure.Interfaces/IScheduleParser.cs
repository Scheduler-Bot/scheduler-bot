using System;

namespace SchedulerBot.Infrastructure.Interfaces
{
	public interface IScheduleParser
	{
		ISchedule Parse(string textSchedule, DateTime baseTime, TimeSpan? timeZoneOffset);

		bool TryParse(string textSchedule, DateTime baseTime, TimeSpan? timeZoneOffset, out ISchedule schedule);
	}
}
