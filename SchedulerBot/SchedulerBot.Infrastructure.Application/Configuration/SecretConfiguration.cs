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
		public SecretConfiguration(IAuthenticationConfiguration authentication)
		{
			Authentication = authentication;
		}

		/// <inheritdoc />
		public string ConnectionString { get; set; }

		/// <inheritdoc />
		public string MicrosoftAppIdKey { get; set; }

		/// <inheritdoc />
		public string MicrosoftAppPassword { get; set; }

		/// <inheritdoc />
		public IAuthenticationConfiguration Authentication { get; set; }
	}
}
