using System;
using SchedulerBot.Infrastructure.Interfaces.Configuration;

namespace SchedulerBot.Infrastructure.Application.Configuration
{
	public class ApplicationConfiguration : IApplicationConfiguration
	{
		public ApplicationConfiguration(
			ISecretConfiguration secrets,
			ICommandConfiguration commands)
		{
			Secrets = secrets;
			Commands = commands;
		}

		public TimeSpan MessageProcessingInterval { get; set; }

		public string ConnectionString { get; set; }

		public ISecretConfiguration Secrets { get; }

		public ICommandConfiguration Commands { get; set; }
	}
}
