using System;

namespace SchedulerBot.Infrastructure.Interfaces.Schedule
{
	public interface IScheduleParser
	{
		ISchedule Parse(string textSchedule, TimeSpan? timeZoneOffset);

		bool TryParse(string textSchedule, TimeSpan? timeZoneOffset, out ISchedule schedule);
	}
}
