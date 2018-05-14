using System;
using SchedulerBot.Infrastructure.Interfaces.Configuration;

namespace SchedulerBot.Infrastructure.Application.Configuration
{
	/// <inheritdoc />
	public class ApplicationConfiguration : IApplicationConfiguration
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ApplicationConfiguration"/> class.
		/// </summary>
		/// <param name="secrets">The secrets.</param>
		/// <param name="commands">The commands.</param>
		public ApplicationConfiguration(
			ISecretConfiguration secrets,
			ICommandConfiguration commands)
		{
			Secrets = secrets;
			Commands = commands;
		}

		/// <inheritdoc />
		public TimeSpan MessageProcessingInterval { get; set; }

		/// <inheritdoc />
		public string ConnectionString { get; set; }

		/// <inheritdoc />
		public ISecretConfiguration Secrets { get; }

		/// <inheritdoc />
		public ICommandConfiguration Commands { get; set; }
	}
}
