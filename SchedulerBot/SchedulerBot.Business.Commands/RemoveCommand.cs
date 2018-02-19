using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchedulerBot.Business.Commands.Utils;
using SchedulerBot.Business.Interfaces;
using SchedulerBot.Business.Interfaces.Entities;
using SchedulerBot.Database.Core;
using SchedulerBot.Database.Entities;

namespace SchedulerBot.Business.Commands
{
	// Expected input: remove '75fc6a1e-f524-4807-81c5-e5b7ab0ac2d0'
	public class RemoveCommand : IBotCommand
	{
		private readonly SchedulerBotContext context;
		private readonly ILogger<RemoveCommand> logger;

		public RemoveCommand(SchedulerBotContext context, ILogger<RemoveCommand> logger)
		{
			this.context = context;
			this.logger = logger;

			Name = "remove";
		}

		public string Name { get; }

		public async Task<CommandExecutionResult> ExecuteAsync(Activity activity, string arguments)
		{
			logger.LogInformation("Executing '{0}' command with arguments '{1}'", Name, arguments);

			CommandExecutionResult result = null;
			string[] splitArguments = ArgumentHelper.ParseArguments(arguments);
			string messageIdText = splitArguments.ElementAtOrDefault(0);

			if (messageIdText != null && Guid.TryParse(messageIdText, out Guid messageId))
			{
				logger.LogInformation("Parsed the arguments to message id '{0}'", messageId);

				ScheduledMessage scheduledMessage = await context
					.ScheduledMessages
					.Where(message => message.Id == messageId)
					.Include(message => message.Details)
					.Include(message => message.Events)
					.FirstOrDefaultAsync();

				if (scheduledMessage != null)
				{
					logger.LogInformation("Removing scheduled message with id '{0}'", messageId);
					context.Remove(scheduledMessage);

					await context.SaveChangesAsync();

					result = "The event has been removed";
					logger.LogInformation("The scheduled message '{0}' has been removed", messageId);
				}
				else
				{
					logger.LogWarning("Scheduled message with id '{0}' cannot be found", messageId);
				}
			}
			else
			{
				logger.LogWarning("Cannot parse the command arguments '{0}'", arguments);
			}

			return result ?? CommandExecutionResult.Error("Cannot remove such an event");
		}
	}
}
