using SchedulerBot.Infrastructure.Interfaces.Application;

namespace SchedulerBot.Infrastructure.Application
{
	/// <inheritdoc />
	public class ApplicationContext : IApplicationContext
	{
		/// <inheritdoc />
		public string Host { get; set; }

		/// <inheritdoc />
		public string Protocol { get; set; }
	}
}
