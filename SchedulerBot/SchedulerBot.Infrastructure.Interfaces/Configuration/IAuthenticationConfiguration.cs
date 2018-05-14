using System;

namespace SchedulerBot.Infrastructure.Interfaces.Configuration
{
	public interface IAuthenticationConfiguration
	{
		string Scheme { get; set; }

		string SigningKey { get; set; }

		string Issuer { get; set; }

		string Audience { get; set; }

		TimeSpan ExpirationPeriod { get; set; }
	}
}
