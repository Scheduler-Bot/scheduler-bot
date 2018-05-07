namespace SchedulerBot.Infrastructure.Interfaces.Application
{
	/// <summary>
	/// Provides properties describing the current application state.
	/// </summary>
	public interface IApplicationContext
	{
		/// <summary>
		/// Gets the application host.
		/// </summary>
		string Host { get; }

		/// <summary>
		/// Gets the protocol being used for accessing the application.
		/// </summary>
		string Protocol { get; }
	}
}
