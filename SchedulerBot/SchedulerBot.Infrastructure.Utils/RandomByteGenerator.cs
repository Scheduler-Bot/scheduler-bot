using System;
using System.Security.Cryptography;
using SchedulerBot.Infrastructure.Interfaces.Utils;

namespace SchedulerBot.Infrastructure.Utils
{
	/// <summary>
	/// Provides a way of generating a cryptographically strong random sequence of bytes.
	/// </summary>
	public sealed class RandomByteGenerator : IRandomByteGenerator, IDisposable
	{
		private readonly RandomNumberGenerator randomNumberGenerator;

		/// <summary>
		/// Initializes a new instance of the <see cref="RandomByteGenerator"/> class.
		/// </summary>
		public RandomByteGenerator()
		{
			randomNumberGenerator = RandomNumberGenerator.Create();
		}

		/// <inheritdoc />
		public byte[] Generate(int length)
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(length));
			}

			byte[] bytes = new byte[length];

			randomNumberGenerator.GetBytes(bytes);

			return bytes;
		}

		/// <inheritdoc />
		public void Dispose()
		{
			randomNumberGenerator.Dispose();
		}
	}
}
