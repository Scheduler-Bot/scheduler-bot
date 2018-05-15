using SchedulerBot.Infrastructure.Interfaces.Configuration;

namespace SchedulerBot.Infrastructure.Application.Configuration
{
	/// <inheritdoc />
	public class SecretConfiguration : ISecretConfiguration
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SecretConfiguration"/> class.
		/// </summary>
		/// <param name="authentication">The authentication.</param>
		/// <param name="microsoftAppCredentials">The Microsoft application credentials.</param>
		public SecretConfiguration(
			IAuthenticationConfiguration authentication,
			IMicrosoftCredentialConfiguration microsoftAppCredentials)
		{
			Authentication = authentication;
			MicrosoftAppCredentials = microsoftAppCredentials;
		}

		/// <inheritdoc />
		public string ConnectionString { get; set; }

		/// <inheritdoc />
		public IAuthenticationConfiguration Authentication { get; }

		/// <inheritdoc />
		public IMicrosoftCredentialConfiguration MicrosoftAppCredentials { get; }
	}
}
