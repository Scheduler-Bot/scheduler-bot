using Microsoft.Bot.Schema;
using SchedulerBot.Business.Entities;

namespace SchedulerBot.Business.Interfaces
{
	/// <summary>
	/// Defines a mechanism of parsing a user request to a concrete command and arguments.
	/// </summary>
	public interface ICommandRequestParser
	{
		/// <summary>
		/// Parses the specified activity to a <see cref="CommandRequestParseResult"/> instance.
		/// </summary>
		/// <param name="activity">The activity.</param>
		/// <returns><see cref="CommandRequestParseResult"/> instance describing which command should be invoked and how.</returns>
		CommandRequestParseResult Parse(Activity activity);
	}
}
