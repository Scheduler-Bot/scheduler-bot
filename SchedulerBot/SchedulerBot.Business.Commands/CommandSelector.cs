using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SchedulerBot.Business.Interfaces;

namespace SchedulerBot.Business.Commands
{
	public class CommandSelector : ICommandSelector
	{
		private const string CommandNameGroup = "name";
		private const string CommandPattern = @"^\s*(?'" + CommandNameGroup + @"'[^\s]+)\s+.*$";
		private static readonly Regex CommandRegex = new Regex(CommandPattern);
		private readonly IList<IBotCommand> commands;

		public CommandSelector(IList<IBotCommand> commands)
		{
			this.commands = commands;
		}

		public IBotCommand GetCommand(string inputText)
		{
			IBotCommand matchingCommand = null;
			Match match = CommandRegex.Match(inputText);

			if (match.Success)
			{
				string commandName = match.Groups[CommandNameGroup].Value;
				matchingCommand = commands.FirstOrDefault(command => command.Name.Equals(commandName, StringComparison.OrdinalIgnoreCase));
			}

			return matchingCommand;
		}
	}
}
