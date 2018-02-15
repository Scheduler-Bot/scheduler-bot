using System;
using System.Collections.Generic;
using System.Linq;
using SchedulerBot.Business.Interfaces;

namespace SchedulerBot.Business.Commands
{
	public class CommandSelector : ICommandSelector
	{
		private readonly IList<IBotCommand> commands;

		public CommandSelector(IList<IBotCommand> commands)
		{
			this.commands = commands;
		}

		public IBotCommand GetCommand(string name)
		{
			return commands.FirstOrDefault(command => command.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
		}
	}
}
