namespace SchedulerBot.Infrastructure.Interfaces.Configuration
{
	/// <summary>
	/// Represents the command configuration.
	/// </summary>
	public interface ICommandConfiguration
	{
		/// <summary>
		/// Gets or sets the manage command configuration.
		/// </summary>
		IManageCommandConfiguration Manage { get; set; }

		/// <summary>
		/// Gets or sets the next.
		/// </summary>
		INextCommandConfiguration Next { get; set; }
	}
}
