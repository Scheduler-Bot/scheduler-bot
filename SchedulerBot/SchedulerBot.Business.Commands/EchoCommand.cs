using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using SchedulerBot.Business.Commands.Utils;
using SchedulerBot.Business.Entities;

namespace SchedulerBot.Business.Commands
{
	/// <summary>
	/// The command for responding to the user with the same text.
	/// </summary>
	/// <example>
	/// Input: echo 'Hello World!'
	/// Output: Hello World!
	/// </example>
	/// <seealso cref="BotCommand" />
	public class EchoCommand : BotCommand
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EchoCommand"/> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		public EchoCommand(ILogger<EchoCommand> logger) : base("echo", logger)
		{
		}

		/// <inheritdoc />
		protected override Task<CommandExecutionResult> ExecuteCoreAsync(Activity activity, string arguments)
		{
			CommandExecutionResult executionResult;

			string[] splitArguments = ArgumentHelper.ParseArguments(arguments);
			string echoText = splitArguments.FirstOrDefault();

			if (echoText != null)
			{
				Logger.LogInformation("Preparing to echo with '{0}'", echoText);
				executionResult = CommandExecutionResult.Success(echoText);
			}
			else
			{
				Logger.LogError("No arguments provided");
				executionResult = CommandExecutionResult.Error("You must provide an argument for this command");
			}

			return Task.FromResult(executionResult);
		}
	}
}
