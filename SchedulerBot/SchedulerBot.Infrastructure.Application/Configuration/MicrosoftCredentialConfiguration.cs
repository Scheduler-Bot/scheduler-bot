using SchedulerBot.Infrastructure.Interfaces.Configuration;

namespace SchedulerBot.Infrastructure.Application.Configuration
{
	/// <inheritdoc />
	public class MicrosoftCredentialConfiguration : IMicrosoftCredentialConfiguration
	{
		/// <inheritdoc />
		public string Id { get; set; }

		/// <inheritdoc />
		public string Password { get; set; }
	}
}
