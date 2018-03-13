namespace SchedulerBot.Business.Entities
{
	/// <summary>
	/// Holds the credentials for accessing the bot service.
	/// </summary>
	public class AppCredentials
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AppCredentials"/> class.
		/// </summary>
		/// <param name="appId">The application identifier.</param>
		/// <param name="password">The password.</param>
		public AppCredentials(string appId, string password)
		{
			AppId = appId;
			AppPassword = password;
		}

		/// <summary>
		/// Gets the application identifier.
		/// </summary>
		public string AppId { get; }

		/// <summary>
		/// Gets the application password.
		/// </summary>
		public string AppPassword { get; }
	}
}
