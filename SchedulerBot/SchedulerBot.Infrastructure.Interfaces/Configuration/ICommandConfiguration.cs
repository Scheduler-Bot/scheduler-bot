namespace SchedulerBot.Infrastructure.Interfaces.Configuration
{
	public interface ICommandConfiguration
	{
		IManageCommandConfiguration Manage { get; set; }

		INextCommandConfiguration Next { get; set; }
	}
}
