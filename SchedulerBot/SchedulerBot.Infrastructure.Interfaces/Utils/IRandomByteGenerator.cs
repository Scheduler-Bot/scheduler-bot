namespace SchedulerBot.Infrastructure.Interfaces.Utils
{
	/// <summary>
	/// Provides a way of generating a cryptographically strong random sequence of bytes.
	/// </summary>
	public interface IRandomByteGenerator
	{
		/// <summary>
		/// Generates a random sequence of bytes of specified length.
		/// </summary>
		/// <param name="length">The length.</param>
		/// <returns>The array of random bytes.</returns>
		byte[] Generate(int length);
	}
}
