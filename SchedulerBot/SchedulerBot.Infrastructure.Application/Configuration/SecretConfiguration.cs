using SchedulerBot.Infrastructure.Interfaces.Configuration;

namespace SchedulerBot.Infrastructure.Application.Configuration
{
	public class SecretConfiguration : ISecretConfiguration
	{
		public SecretConfiguration(IAuthenticationConfiguration authentication)
		{
			Authentication = authentication;
		}

		public string ConnectionString { get; set; }

		public string MicrosoftAppId { get; set; }

		public string MicrosoftAppPassword { get; set; }

		public IAuthenticationConfiguration Authentication { get; set; }
	}
}
