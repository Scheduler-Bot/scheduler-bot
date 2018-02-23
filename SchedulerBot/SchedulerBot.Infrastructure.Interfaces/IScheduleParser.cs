using System;

namespace SchedulerBot.Infrastructure.Interfaces
{
	public interface IScheduleParser
	{
		ISchedule Parse(string textSchedule, TimeSpan? timeZoneOffset);

		bool TryParse(string textSchedule, TimeSpan? timeZoneOffset, out ISchedule schedule);
	}
}
