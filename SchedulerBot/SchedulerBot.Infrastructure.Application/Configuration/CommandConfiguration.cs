using SchedulerBot.Infrastructure.Interfaces.Configuration;

namespace SchedulerBot.Infrastructure.Application.Configuration
{
	public class CommandConfiguration : ICommandConfiguration
	{
		public CommandConfiguration(
			IManageCommandConfiguration manage,
			INextCommandConfiguration next)
		{
			Manage = manage;
			Next = next;
		}

		public IManageCommandConfiguration Manage { get; set; }

		public INextCommandConfiguration Next { get; set; }
	}
}
