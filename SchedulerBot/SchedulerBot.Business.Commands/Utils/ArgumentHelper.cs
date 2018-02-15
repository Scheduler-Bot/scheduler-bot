using System.Linq;
using System.Text.RegularExpressions;

namespace SchedulerBot.Business.Commands.Utils
{
	internal static class ArgumentHelper
	{
		// input: not in quotes 'in single quotes' "in double quotes"
		// output: not, in, quotes, in single quotes, in double quotes
		private static readonly Regex ArgumentRegex = new Regex("(?<=\")[^\"]*(?=\")|(?<=\')[^\']*(?=\')|[^\\s\"\']+");

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
