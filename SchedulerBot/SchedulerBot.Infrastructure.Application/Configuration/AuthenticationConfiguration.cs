using System;
using SchedulerBot.Infrastructure.Interfaces.Configuration;

namespace SchedulerBot.Infrastructure.Application.Configuration
{
	public class AuthenticationConfiguration : IAuthenticationConfiguration
	{
		public string Scheme { get; set; }

		public string SigningKey { get; set; }

		public string Issuer { get; set; }

		public string Audience { get; set; }

		public TimeSpan ExpirationPeriod { get; set; }
	}
}
