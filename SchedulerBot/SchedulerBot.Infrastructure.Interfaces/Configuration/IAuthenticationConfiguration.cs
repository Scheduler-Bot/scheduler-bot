using System;

namespace SchedulerBot.Infrastructure.Interfaces.Configuration
{
	/// <summary>
	/// Represents the authentication configuration.
	/// </summary>
	public interface IAuthenticationConfiguration
	{
		/// <summary>
		/// Gets or sets the authentication scheme.
		/// </summary>
		string Scheme { get; set; }

		/// <summary>
		/// Gets or sets the authentication token signing key.
		/// </summary>
		string SigningKey { get; set; }

		/// <summary>
		/// Gets or sets the authentication token issuer.
		/// </summary>
		string Issuer { get; set; }

		/// <summary>
		/// Gets or sets the authentication token audience.
		/// </summary>
		string Audience { get; set; }

		/// <summary>
		/// Gets or sets the authentication token expiration period.
		/// </summary>
		TimeSpan ExpirationPeriod { get; set; }
	}
}
