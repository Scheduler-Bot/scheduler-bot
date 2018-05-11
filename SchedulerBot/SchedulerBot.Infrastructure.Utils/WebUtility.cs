using System;
using System.Text;
using SchedulerBot.Infrastructure.Interfaces.Utils;

namespace SchedulerBot.Infrastructure.Utils
{
	/// <inheritdoc />
	public class WebUtility : IWebUtility
	{
		#region Private Fields

		private readonly IRandomByteGenerator randomByteGenerator;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="WebUtility"/> class.
		/// </summary>
		/// <param name="randomByteGenerator">The random byte generator.</param>
		public WebUtility(IRandomByteGenerator randomByteGenerator)
		{
			this.randomByteGenerator = randomByteGenerator;
		}

		#endregion

		#region Implementation of IWebUtility

		/// <inheritdoc />
		public string GenerateRandomUrlCompatibleString(int length)
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(length));
			}

			string randomBase64String = GetRandomBase64String(length);
			string randomUrlEncodedString = UrlEncodeBase64String(randomBase64String, length);

			return randomUrlEncodedString;
		}

		/// <inheritdoc />
		public string GenerateUrl(string protocol, string host, params string[] routeParts)
		{
			StringBuilder stringBuilder = new StringBuilder();

			stringBuilder
				.Append(protocol)
				.Append("://")
				.Append(host);

			foreach (string routePart in routeParts)
			{
				stringBuilder
					.Append('/')
					.Append(routePart);
			}

			return stringBuilder.ToString();
		}

		#endregion

		#region Private Methods

		private string GetRandomBase64String(int minLength)
		{
			byte[] randomBytes = randomByteGenerator.Generate(minLength);
			string randomBase64String = Convert.ToBase64String(randomBytes);

			return randomBase64String;
		}

		private static string UrlEncodeBase64String(string base64String, int length)
		{
			// Get rid of '=' chars at the end
			while (length > 0 && base64String[length - 1] == '=')
			{
				--length;
			}

			StringBuilder stringBuilder = new StringBuilder();

			for (int i = 0; i < length; i++)
			{
				char currentCharacter = base64String[i];
				char characterToAppend;

				switch (currentCharacter)
				{
					// '+' is not safe in url, so replace it with '-'
					case '+':
						characterToAppend = '-';
						break;
					// '/' is not safe in url, so replace it with '_'
					case '/':
						characterToAppend = '_';
						break;
					default:
						characterToAppend = currentCharacter;
						break;
				}

				stringBuilder.Append(characterToAppend);
			}

			return stringBuilder.ToString();
		}

		#endregion
	}
}
