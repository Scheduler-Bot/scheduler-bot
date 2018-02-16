using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using SchedulerBot.Business.Interfaces;

namespace SchedulerBot.Business.Commands.Utils
{
	public class CommandSelector : ICommandSelector
	{
		private readonly IList<IBotCommand> commands;
		private readonly ILogger<CommandSelector> logger;

		public CommandSelector(IList<IBotCommand> commands, ILogger<CommandSelector> logger)
		{
			this.commands = commands;
			this.logger = logger;
		}

		public IBotCommand GetCommand(string name)
		{
			logger.LogInformation("Trying to select a command by name '{0}'", name);

			IBotCommand foundCommand = commands.FirstOrDefault(command => command.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

			if (foundCommand != null)
			{
				logger.LogInformation("Found the '{0}' command", foundCommand.GetType());
			}
			else
			{
				logger.LogWarning("Cannot find a command with the name '{0}'", name);
			}

			return foundCommand;
		}
	}
}
