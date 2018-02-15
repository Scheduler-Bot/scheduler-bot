using System.Text.RegularExpressions;
using SchedulerBot.Business.Interfaces;

namespace SchedulerBot.Business.Commands.Utils
{
	public class CommandRequestParser : ICommandRequestParser
	{
		private const string NameGroup = "name";
		private const string ArgumentsGroup = "arguments";
		private const string Pattern = @"^\s*(?'" + NameGroup + @"'[^\s]+)\s*(?'" + ArgumentsGroup + @"'.*)\s*$";
		private static readonly Regex CommandRegex = new Regex(Pattern);

		public CommandRequestParseResult Parse(string inputText)
		{
			CommandRequestParseResult parseResult = null;
			Match match = CommandRegex.Match(inputText);

			if (match.Success)
			{
				string name = match.Groups[NameGroup].Value;
				string arguments = match.Groups[ArgumentsGroup]?.Value ?? string.Empty;

				parseResult = new CommandRequestParseResult(name, arguments);
			}

			return parseResult;
		}
	}
}
