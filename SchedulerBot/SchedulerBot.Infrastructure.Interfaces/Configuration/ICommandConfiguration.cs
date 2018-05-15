namespace SchedulerBot.Infrastructure.Interfaces.Configuration
{
	/// <summary>
	/// Represents the command configuration.
	/// </summary>
	public interface ICommandConfiguration
	{
		/// <summary>
		/// Gets the manage command configuration.
		/// </summary>
		IManageCommandConfiguration Manage { get; }

		/// <summary>
		/// Gets the next command configuration.
		/// </summary>
		INextCommandConfiguration Next { get; }
	}
}
