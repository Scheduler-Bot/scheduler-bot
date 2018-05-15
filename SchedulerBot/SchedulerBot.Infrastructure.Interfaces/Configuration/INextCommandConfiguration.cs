namespace SchedulerBot.Infrastructure.Interfaces.Configuration
{
	/// <summary>
	/// Represents the next command configuration.
	/// </summary>
	public interface INextCommandConfiguration
	{
		/// <summary>
		/// Gets or sets the maximum message count.
		/// </summary>
		int MaxMessageCount { get; set; }

		/// <summary>
		/// Gets or sets the default message count.
		/// </summary>
		int DefaultMessageCount { get; set; }
	}
}
