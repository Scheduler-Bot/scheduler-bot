using SchedulerBot.Infrastructure.Interfaces.Configuration;

namespace SchedulerBot.Infrastructure.Application.Configuration
{
	public class NextCommandConfiguration : INextCommandConfiguration
	{
		public int MaxMessageCount { get; set; }

		public int DefaultMessageCount { get; set; }
	}
}
