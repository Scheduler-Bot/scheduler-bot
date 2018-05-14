using System;

namespace SchedulerBot.Infrastructure.Interfaces.Configuration
{
	public interface IApplicationConfiguration
	{
		TimeSpan MessageProcessingInterval { get; set; }

		string ConnectionString { get; set; }

		ISecretConfiguration Secrets { get; }

		ICommandConfiguration Commands { get; set; }
	}
}
