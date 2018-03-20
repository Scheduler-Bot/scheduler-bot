using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchedulerBot.Business.Commands.Utils;
using SchedulerBot.Business.Entities;
using SchedulerBot.Database.Core;
using SchedulerBot.Database.Entities;
using SchedulerBot.Database.Entities.Enums;

namespace SchedulerBot.Business.Commands
{
	/// <summary>
	/// The command allowing to remove a scheduled message.
	/// </summary>
	/// <example>Expected input: remove '75fc6a1e-f524-4807-81c5-e5b7ab0ac2d0'</example>
	/// <seealso cref="BotCommand" />
	public class RemoveCommand : BotCommand
	{
		private readonly SchedulerBotContext context;

		/// <summary>
		/// Initializes a new instance of the <see cref="RemoveCommand"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="logger">The logger.</param>
		public RemoveCommand(SchedulerBotContext context, ILogger<RemoveCommand> logger)
			: base("remove", logger)
		{
			this.context = context;
		}

		/// <inheritdoc />
		protected override async Task<CommandExecutionResult> ExecuteCoreAsync(Activity activity, string arguments)
		{
			CommandExecutionResult result = null;
			string[] splitArguments = ArgumentHelper.ParseArguments(arguments);
			string messageIdText = splitArguments.ElementAtOrDefault(0);

			if (messageIdText != null && Guid.TryParse(messageIdText, out Guid messageId))
			{
				Logger.LogInformation("Parsed the arguments to message id '{0}'", messageId);

				ScheduledMessage scheduledMessage = await context
					.ScheduledMessages
					.Where(message => message.Id == messageId && message.State == ScheduledMessageState.Active)
					.Include(message => message.Details)
					.Include(message => message.Events)
					.FirstOrDefaultAsync();

				if (scheduledMessage != null)
				{
					Logger.LogInformation("Removing scheduled message with id '{0}'", messageId);
					scheduledMessage.State = ScheduledMessageState.Deleted;
					context.Update(scheduledMessage);

					foreach (ScheduledMessageEvent scheduledMessageEvent
						in scheduledMessage.Events.Where(@event => @event.State == ScheduledMessageEventState.Pending))
					{
						Logger.LogInformation("Removing scheduled message event with id '{0}'", scheduledMessageEvent.Id);
						scheduledMessageEvent.State = ScheduledMessageEventState.Deleted;
						// TODO: Do we need this update operation below?
						context.Update(scheduledMessageEvent);
					}

					await context.SaveChangesAsync();

					result = "The event has been removed";
					Logger.LogInformation("The scheduled message '{0}' has been removed", messageId);
				}
				else
				{
					Logger.LogWarning("Scheduled message with id '{0}' cannot be found", messageId);
				}
			}
			else
			{
				Logger.LogWarning("Cannot parse the command arguments '{0}'", arguments);
			}

			return result ?? CommandExecutionResult.Error("Cannot remove such an event");
		}
	}
}
