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
		/// Gets or sets the Microsoft application identifier.
		/// </summary>
		string MicrosoftAppIdKey { get; set; }

		/// <summary>
		/// Gets or sets the Microsoft application password.
		/// </summary>
		string MicrosoftAppPassword { get; set; }

		/// <summary>
		/// Gets or sets the authentication configuration.
		/// </summary>
		IAuthenticationConfiguration Authentication { get; set; }
	}
}
