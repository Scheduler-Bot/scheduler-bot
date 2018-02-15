using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace SchedulerBot.Business.Commands.Utils
{
	internal static class ArgumentHelper
	{
		public static string Unquote(string argument) => argument.Trim('\'', '"');

		public static string[] ParseArguments(string arguments)
		{
			return Regex
				.Split(arguments, @"\s")
				.Where(argument => string.Empty.Equals(argument, StringComparison.Ordinal))
				.Select(Unquote)
				.ToArray();
		}
	}
}
