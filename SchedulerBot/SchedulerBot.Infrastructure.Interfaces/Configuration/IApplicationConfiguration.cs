using System;

namespace SchedulerBot.Infrastructure.Interfaces.Configuration
{
	/// <summary>
	/// Represents the application configuration.
	/// </summary>
	public interface IApplicationConfiguration
	{
		/// <summary>
		/// Gets or sets the message processing interval.
		/// </summary>
		TimeSpan MessageProcessingInterval { get; set; }

		/// <summary>
		/// Gets or sets the connection string.
		/// </summary>
		string ConnectionString { get; set; }

		/// <summary>
		/// Gets the secret configuration.
		/// </summary>
		ISecretConfiguration Secrets { get; }

		/// <summary>
		/// Gets or sets the command configuration.
		/// </summary>
		ICommandConfiguration Commands { get; set; }
	}
}
