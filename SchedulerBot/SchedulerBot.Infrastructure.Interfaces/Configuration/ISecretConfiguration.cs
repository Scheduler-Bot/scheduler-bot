namespace SchedulerBot.Infrastructure.Interfaces.Configuration
{
	public interface ISecretConfiguration
	{
		string ConnectionString { get; set; }

		string MicrosoftAppId { get; set; }

		string MicrosoftAppPassword { get; set; }

		IAuthenticationConfiguration Authentication { get; set; }
	}
}
