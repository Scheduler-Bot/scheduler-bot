using SchedulerBot.Infrastructure.Interfaces.Configuration;

namespace SchedulerBot.Infrastructure.Application.Configuration
{
	/// <inheritdoc />
	public class NextCommandConfiguration : INextCommandConfiguration
	{
		/// <inheritdoc />
		public int MaxMessageCount { get; set; }

		/// <inheritdoc />
		public int DefaultMessageCount { get; set; }
	}
}
