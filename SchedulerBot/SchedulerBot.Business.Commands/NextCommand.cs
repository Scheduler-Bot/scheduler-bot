using System;
using System.Globalization;
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
using SchedulerBot.Database.Entities.Enums;

namespace SchedulerBot.Business.Commands
{
	public class NextCommand : IBotCommand
	{
		private readonly SchedulerBotContext context;
		private readonly IMessageListFormatter messageListFormatter;
		private readonly ILogger<ListCommand> logger;

		public NextCommand(
			SchedulerBotContext context,
			IMessageListFormatter messageListFormatter,
			ILogger<ListCommand> logger)
		{
			this.context = context;
			this.messageListFormatter = messageListFormatter;
			this.logger = logger;

			Name = "next";
		}

		public string Name { get; }

		public Task<CommandExecutionResult> ExecuteAsync(Activity activity, string arguments)
		{
			CommandExecutionResult result;

			logger.LogInformation("Executing '{0}' command", Name);

			if (TryGetMessageCount(activity, arguments, out int messageCount))
			{
				IQueryable<ScheduledMessage> conversationMessages = GetConversationMessages(activity.Conversation.Id, messageCount);
				string messages = messageListFormatter.Format(conversationMessages, activity);

				result = CommandExecutionResult.Success(messages);
			}
			else
			{
				logger.LogWarning($"Cannot parse the command arguments '{arguments}'");
				result = CommandExecutionResult.Error("Cannot recognize the command arguments");
			}

			return Task.FromResult(result);
		}

		private static bool TryGetMessageCount(Activity activity, string arguments, out int messageCount)
		{
			bool result = true;

			if (!string.IsNullOrWhiteSpace(arguments))
			{
				string[] splitArguments = ArgumentHelper.ParseArguments(arguments);
				string stringMessageCount = splitArguments.FirstOrDefault();
				CultureInfo cultureInfo = GetCultureInfoOrDefault(activity.Locale);

				if (!int.TryParse(stringMessageCount, NumberStyles.Integer, cultureInfo, out messageCount))
				{
					result = false;
				}
			}
			else
			{
				messageCount = 1;
			}

			return result;
		}

		private static CultureInfo GetCultureInfoOrDefault(string name)
		{
			CultureInfo cultureInfo;

			try
			{
				cultureInfo = CultureInfo.GetCultureInfo(name);
			}
			catch (CultureNotFoundException)
			{
				cultureInfo = CultureInfo.InvariantCulture;
			}

			return cultureInfo;
		}

		private IQueryable<ScheduledMessage> GetConversationMessages(string conversationId, int count)
		{
			return context
				.ScheduledMessageEvents
				.Include(@event => @event.ScheduledMessage)
				.ThenInclude(message => message.Details)
				.Where(@event => @event.State == ScheduledMessageEventState.Pending)
				.OrderBy(@event => @event.NextOccurence)
				.Select(@event => @event.ScheduledMessage)
				.Where(message =>
					message.State == ScheduledMessageState.Active &&
					message.Details.ConversationId.Equals(conversationId, StringComparison.Ordinal))
				.Take(count);
		}
	}
}
