using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using SchedulerBot.Business.Entities;

namespace SchedulerBot.Business.Commands
{
	/// <summary>
	/// The command allowing for a user to get a link to a temporary page
	/// where they can manage the events for the current conversation.
	/// </summary>
	/// <seealso cref="BotCommand" />
	public class ManageCommand : BotCommand
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ManageCommand"/> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		public ManageCommand(ILogger<ManageCommand> logger) : base("manage", logger)
		{
		}

		/// <inheritdoc />
		protected override Task<CommandExecutionResult> ExecuteCoreAsync(Activity activity, string arguments)
		{
			CommandExecutionResult executionResult = CommandExecutionResult.Success("This is supposed to be a link");

			return Task.FromResult(executionResult);
		}
	}
}
