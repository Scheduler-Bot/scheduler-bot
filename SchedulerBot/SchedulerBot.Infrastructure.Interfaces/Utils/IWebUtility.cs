namespace SchedulerBot.Infrastructure.Interfaces.Utils
{
	/// <summary>
	/// Provides helper methods for working with URL addresses, etc.
	/// </summary>
	public interface IWebUtility
	{
		/// <summary>
		/// Generates the random URL compatible string.
		/// </summary>
		/// <param name="length">The length.</param>
		/// <returns>A random string consisting from URL compatible characters.</returns>
		string GenerateRandomUrlCompatibleString(int length);

		/// <summary>
		/// Generates the URL.
		/// </summary>
		/// <param name="protocol">The protocol.</param>
		/// <param name="host">The host.</param>
		/// <param name="routeParts">The route parts. Will be separated with slashes in the resulting URL.</param>
		/// <returns>The generated URL.</returns>
		string GenerateUrl(string protocol, string host, params string[] routeParts);
	}
}
