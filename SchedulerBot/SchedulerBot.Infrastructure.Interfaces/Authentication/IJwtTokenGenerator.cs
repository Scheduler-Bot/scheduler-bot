using System.Collections.Generic;

namespace SchedulerBot.Infrastructure.Interfaces.Authentication
{
	/// <summary>
	/// Provides a way to generate a JWT token.
	/// </summary>
	public interface IJwtTokenGenerator
	{
		/// <summary>
		/// Generates the JWT token.
		/// </summary>
		/// <param name="claims">The claims to be included into a resulting token.</param>
		/// <returns>The generated JWT token.</returns>
		string GenerateToken(IDictionary<string, string> claims);
	}
}
