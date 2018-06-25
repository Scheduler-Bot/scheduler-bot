using Microsoft.Bot.Schema;
using SchedulerBot.Business.Entities;

namespace SchedulerBot.Business.Interfaces.Commands
{
	/// <summary>
	/// Defines a mechanism of parsing a user request to a concrete command and arguments.
	/// </summary>
	public interface ICommandRequestParser
	{
		/// <summary>
		/// Parses the specified activity to a <see cref="ParsedCommandRequest"/> instance.
		/// </summary>
		/// <param name="activity">The activity.</param>
		/// <returns><see cref="ParsedCommandRequest"/> instance describing which command should be invoked and how.</returns>
		ParsedCommandRequest Parse(Activity activity);
	}
}
