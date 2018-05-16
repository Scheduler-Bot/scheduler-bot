namespace SchedulerBot.Infrastructure.Interfaces.Configuration
{
	/// <summary>
	/// Represents the secret configuration.
	/// </summary>
	public interface ISecretConfiguration
	{
		/// <summary>
		/// Gets or sets the connection string.
		/// </summary>
		string ConnectionString { get; set; }

		/// <summary>
		/// Gets the authentication configuration.
		/// </summary>
		IAuthenticationConfiguration Authentication { get; }

		/// <summary>
		/// Gets the Microsoft application credentials.
		/// </summary>
		IMicrosoftCredentialConfiguration MicrosoftAppCredentials { get; }
	}
}
