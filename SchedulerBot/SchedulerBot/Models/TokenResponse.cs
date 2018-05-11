namespace SchedulerBot.Models
{
	/// <summary>
	/// Encapsulates a response containing an authentication token.
	/// </summary>
	public class TokenResponse
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TokenResponse"/> class.
		/// </summary>
		/// <param name="token">The authentication token.</param>
		public TokenResponse(string token)
		{
			Token = token;
		}

		/// <summary>
		/// Gets the authentication token.
		/// </summary>
		public string Token { get; }
	}
}
