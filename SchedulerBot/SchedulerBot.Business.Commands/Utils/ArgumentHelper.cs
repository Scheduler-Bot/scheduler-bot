using System.Linq;
using System.Text.RegularExpressions;

namespace SchedulerBot.Business.Commands.Utils
{
	/// <summary>
	/// Provides helper methods for the command argument parsing process.
	/// </summary>
	internal static class ArgumentHelper
	{
		// input: not in quotes 'in single quotes' "in double quotes"
		// output: not, in, quotes, in single quotes, in double quotes
		private static readonly Regex ArgumentRegex = new Regex("(?<=\")[^\"]*(?=\")|(?<=\')[^\']*(?=\')|[^\\s\"\']+");

		/// <summary>
		/// Parses the argument string into the array of arguments.
		/// </summary>
		/// <param name="arguments">The argument string.</param>
		/// <returns>The array of arguments.</returns>
		public static string[] ParseArguments(string arguments)
		{
			return ArgumentRegex
				.Matches(arguments)
				.Select(match => match.Value.Trim())
				.Where(value => !string.IsNullOrEmpty(value))
				.ToArray();
		}
	}
}
