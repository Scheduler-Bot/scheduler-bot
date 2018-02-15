using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using Microsoft.EntityFrameworkCore;
using SchedulerBot.Business.Interfaces;
using SchedulerBot.Database.Core;
using SchedulerBot.Database.Entities;

namespace SchedulerBot.Business.Commands
{
	public class RemoveCommand : IBotCommand
	{
		private readonly SchedulerBotContext context;

		public RemoveCommand(SchedulerBotContext context)
		{
			this.context = context;

			Name = "remove";
		}

		public string Name { get; }

		public async Task<string> ExecuteAsync(Activity activity, string arguments)
		{
			string result = null;
			string messageIdText = arguments;

			if (Guid.TryParse(messageIdText, out Guid messageId))
			{
				ScheduledMessage scheduledMessage = await context
					.ScheduledMessages
					.Where(message => message.Id == messageId)
					.Include(message => message.Details)
					.Include(message => message.Events)
					.FirstOrDefaultAsync();

				if (scheduledMessage != null)
				{
					context.Remove(scheduledMessage);

					await context.SaveChangesAsync();

					result = "The event has been removed";
				}
			}

			return result ?? "Cannot remove such an event";
		}
	}
}
