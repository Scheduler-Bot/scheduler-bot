using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using SchedulerBot.Business.Commands.Utils;
using SchedulerBot.Business.Entities;
using SchedulerBot.Business.Interfaces;

namespace SchedulerBot.Business.Commands
{
	public class EchoCommand : IBotCommand
	{
		private readonly ILogger<EchoCommand> logger;

		public EchoCommand(ILogger<EchoCommand> logger)
		{
			this.logger = logger;
			Name = "echo";
		}

		public string Name { get; }

		public Task<CommandExecutionResult> ExecuteAsync(Activity activity, string arguments)
		{
			logger.LogInformation("Executing '{0}' command with arguments '{1}'", Name, arguments);

			CommandExecutionResult executionResult;

			string[] splitArguments = ArgumentHelper.ParseArguments(arguments);
			string echoText = splitArguments.FirstOrDefault();

			if (echoText != null)
			{
				logger.LogInformation("Preparing to echo with '{0}'", echoText);
				executionResult = CommandExecutionResult.Success(echoText);
			}
			else
			{
				logger.LogError("No arguments provided");
				executionResult = CommandExecutionResult.Error("You must provide an argument for this command");
			}

			return Task.FromResult(executionResult);
		}
	}
}
