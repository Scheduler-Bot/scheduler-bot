namespace SchedulerBot.Infrastructure.Interfaces.Configuration
{
	public interface INextCommandConfiguration
	{
		int MaxMessageCount { get; set; }

		int DefaultMessageCount { get; set; }
	}
}
