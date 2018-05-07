using SchedulerBot.Infrastructure.Interfaces.Application;

namespace SchedulerBot.Infrastructure.Application
{
	/// <inheritdoc />
	public class ApplicationContext : IApplicationContext
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ApplicationContext"/> class.
		/// </summary>
		/// <param name="host">The host.</param>
		/// <param name="protocol">The protocol.</param>
		public ApplicationContext(string host, string protocol)
		{
			Host = host;
			Protocol = protocol;
		}

		/// <inheritdoc />
		public string Host { get; }

		/// <inheritdoc />
		public string Protocol { get; }
	}
}
