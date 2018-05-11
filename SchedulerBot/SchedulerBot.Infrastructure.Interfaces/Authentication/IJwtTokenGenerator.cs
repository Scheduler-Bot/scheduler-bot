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
		/// <param name="username">The username.</param>
		/// <returns>The generated JWT token.</returns>
		string GenerateToken(string username);
	}
}
