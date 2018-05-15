namespace SchedulerBot.Infrastructure.Interfaces.Configuration
{
	/// <summary>
	/// Represents the Microsoft application credential configuration.
	/// </summary>
	public interface IMicrosoftCredentialConfiguration
	{
		/// <summary>
		/// Gets or sets the application identifier.
		/// </summary>
		string Id { get; set; }

		/// <summary>
		/// Gets or sets the application password.
		/// </summary>
		string Password { get; set; }
	}
}
