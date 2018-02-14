namespace SchedulerBot.Infrastructure.Interfaces
{
	public interface IScheduleParser
	{
		ISchedule Parse(string textSchedule);

		bool TryParse(string textSchedule, out ISchedule schedule);
	}
}
