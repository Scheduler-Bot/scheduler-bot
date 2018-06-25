using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using SchedulerBot.Business.Entities;

namespace SchedulerBot.Business.Interfaces.Commands
{
	/// <summary>
	/// Defines a command used for the interaction between the user and the bot.
	/// </summary>
	public interface IBotCommand
	{
		/// <summary>
		/// Gets the command name.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Executes the command asynchronously.
		/// </summary>
		/// <param name="activity">The activity.</param>
		/// <param name="arguments">The command arguments.</param>
		/// <returns>The result of the command execution.</returns>
		Task<CommandExecutionResult> ExecuteAsync(Activity activity, string arguments);
	}
}
