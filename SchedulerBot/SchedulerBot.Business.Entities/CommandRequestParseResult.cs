﻿namespace SchedulerBot.Business.Entities
{
	public class CommandRequestParseResult
	{
		public CommandRequestParseResult(string name, string arguments)
		{
			Name = name;
			Arguments = arguments;
		}

		public string Name { get; }

		public string Arguments { get; }
	}
}
