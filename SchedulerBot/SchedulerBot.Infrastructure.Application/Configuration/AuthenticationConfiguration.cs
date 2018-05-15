using System;
using SchedulerBot.Infrastructure.Interfaces.Configuration;

namespace SchedulerBot.Infrastructure.Application.Configuration
{
	/// <inheritdoc />
	public class AuthenticationConfiguration : IAuthenticationConfiguration
	{
		/// <inheritdoc />
		public string Scheme { get; set; }

		/// <inheritdoc />
		public string SigningKey { get; set; }

		/// <inheritdoc />
		public string Issuer { get; set; }

		/// <inheritdoc />
		public string Audience { get; set; }

		/// <inheritdoc />
		public TimeSpan ExpirationPeriod { get; set; }
	}
}
