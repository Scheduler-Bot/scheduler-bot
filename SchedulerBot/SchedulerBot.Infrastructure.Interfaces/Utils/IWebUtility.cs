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
		string GenerateRandomUrlCompatibleString(int length);
	}
}
