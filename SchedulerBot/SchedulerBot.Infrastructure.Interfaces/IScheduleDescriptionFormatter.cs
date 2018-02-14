namespace SchedulerBot.Infrastructure.Interfaces
{
	public interface IScheduleDescriptionFormatter
	{
		string Format(ISchedule schedule, string locale);
	}
}
