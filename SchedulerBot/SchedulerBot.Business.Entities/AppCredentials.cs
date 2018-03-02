namespace SchedulerBot.Business.Entities
{
	public class AppCredentials
	{
		public AppCredentials(string appId, string password)
		{
			AppId = appId;
			AppPassword = password;
		}

		public string AppId { get; }

		public string AppPassword { get; }
	}
}
