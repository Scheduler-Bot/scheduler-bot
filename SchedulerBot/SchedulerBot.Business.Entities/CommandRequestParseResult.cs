﻿namespace SchedulerBot.Business.Entities
{
	/// <summary>
	/// Holds the result of parsing the user request to the information about the corresponding command.
	/// </summary>
	public class CommandRequestParseResult
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CommandRequestParseResult"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="arguments">The arguments.</param>
		public CommandRequestParseResult(string name, string arguments)
		{
			Name = name;
			Arguments = arguments;
		}

		/// <summary>
		/// Gets the command name.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Gets the command arguments.
		/// </summary>
		public string Arguments { get; }
	}
}
