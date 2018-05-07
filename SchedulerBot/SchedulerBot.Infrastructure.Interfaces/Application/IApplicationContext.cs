namespace SchedulerBot.Infrastructure.Interfaces.Application
{
	/// <summary>
	/// Provides properties describing the current application state.
	/// </summary>
	public interface IApplicationContext
	{
		/// <summary>
		/// Gets or sets the application host.
		/// </summary>
		string Host { get; set; }

		/// <summary>
		/// Gets or sets the protocol being used for accessing the application.
		/// </summary>
		string Protocol { get; set; }
	}
}
