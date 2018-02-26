using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchedulerBot.Business.Commands.Utils;
using SchedulerBot.Business.Interfaces;
using SchedulerBot.Business.Interfaces.Entities;
using SchedulerBot.Business.Utils;
using SchedulerBot.Database.Core;
using SchedulerBot.Database.Entities;
using SchedulerBot.Database.Entities.Enums;
using SchedulerBot.Infrastructure.Interfaces;

namespace SchedulerBot.Business.Commands
{
	public class NextCommand : IBotCommand
	{
		#region Private Fields

		private readonly int defaultMessageCount;
		private readonly int maxMessageCount;
		private readonly SchedulerBotContext context;
		private readonly IScheduleParser scheduleParser;
		private readonly ILogger<ListCommand> logger;

		#endregion

		#region Constructor

		public NextCommand(
			SchedulerBotContext context,
			IScheduleParser scheduleParser,
			IConfiguration configuration,
			ILogger<ListCommand> logger)
		{
			this.context = context;
			this.scheduleParser = scheduleParser;
			this.logger = logger;

			defaultMessageCount = int.Parse(configuration["Commands:Next:DefaultMessageCount"], CultureInfo.InvariantCulture);
			maxMessageCount = int.Parse(configuration["Commands:Next:MaxMessageCount"], CultureInfo.InvariantCulture);
			Name = "next";
		}

		#endregion

		#region Implementation of IBotCommand

		public string Name { get; }

		public Task<CommandExecutionResult> ExecuteAsync(Activity activity, string arguments)
		{
			logger.LogInformation("Executing '{0}' command", Name);

			CultureInfo clientCulture = GetCultureInfoOrDefault(activity.Locale);
			CommandExecutionResult result = string.IsNullOrWhiteSpace(arguments)
				? ExecuteWithNoArguments(activity, clientCulture)
				: ExecuteWithArguments(activity, arguments, clientCulture);

			return Task.FromResult(result);
		}

		#endregion

		#region Private Methods

		private CommandExecutionResult ExecuteWithNoArguments(Activity activity, CultureInfo clientCulture)
		{
			ScheduledMessageEvent nextEvent = GetNextMessageEvent(activity.Conversation.Id);
			CommandExecutionResult executionResult = nextEvent != null
				? ExecuteWithMessageAndCount(nextEvent.ScheduledMessage, defaultMessageCount, clientCulture)
				: GetNoScheduledMessagesResult();

			return executionResult;
		}

		private CommandExecutionResult ExecuteWithArguments(
			Activity activity,
			string arguments,
			CultureInfo clientCulture)
		{
			CommandExecutionResult result;
			string[] splitArguments = ArgumentHelper.ParseArguments(arguments);
			string messageIdArgument = splitArguments.ElementAtOrDefault(0);

			if (Guid.TryParse(messageIdArgument, out Guid messageId))
			{
				ScheduledMessage scheduledMessage = GetMessageById(activity.Conversation.Id, messageId);

				if (scheduledMessage != null)
				{
					string countArgument = splitArguments.ElementAtOrDefault(1);

					if (countArgument != null)
					{
						result = int.TryParse(countArgument, NumberStyles.Integer, clientCulture, out int count)
							? ExecuteWithMessageAndCount(scheduledMessage, count, clientCulture)
							: CommandExecutionResult.Error($"Cannot retrieve the requested number of events from the argument '{countArgument}'");
					}
					else
					{
						result = ExecuteWithMessage(scheduledMessage, clientCulture);
					}
				}
				else
				{
					result = GetNoScheduledMessagesResult();
				}
			}
			else
			{
				result = CommandExecutionResult.Error($"Cannot parse the message id '{messageIdArgument}'");
			}

			return result;
		}

		private CommandExecutionResult ExecuteWithMessage(ScheduledMessage message, CultureInfo clientCulture)
		{
			ScheduledMessageEvent nextEvent = message
				.Events
				.Where(@event => @event.State == ScheduledMessageEventState.Pending)
				.OrderBy(@event => @event.NextOccurence)
				.FirstOrDefault();
			CommandExecutionResult executionResult = nextEvent != null
				? ExecuteWithMessageAndCount(nextEvent.ScheduledMessage, defaultMessageCount, clientCulture)
				: GetNoScheduledMessagesResult();

			return executionResult;
		}

		private CommandExecutionResult ExecuteWithMessageAndCount(ScheduledMessage message, int count, CultureInfo clientCulture)
		{
			CommandExecutionResult executionResult;

			if (IsRequestedCountValid(count))
			{
				bool hasPendingEvents = message
					.Events
					.Any(@event => @event.State == ScheduledMessageEventState.Pending);

				if (hasPendingEvents)
				{
					ISchedule schedule = scheduleParser.Parse(message.Schedule, message.Details.TimeZoneOffset);
					IEnumerable<DateTime> messageOccurences = schedule
						.GetNextOccurences(DateTime.UtcNow, DateTime.MaxValue)
						.Take(count);

					executionResult = BuildResponseMessageText(message, messageOccurences, clientCulture);
				}
				else
				{
					executionResult = GetNoScheduledMessagesResult();
				}
			}
			else
			{
				executionResult = CommandExecutionResult.Error($"The requested message count must be between 1 and {maxMessageCount}");
			}

			return executionResult;
		}

		private ScheduledMessageEvent GetNextMessageEvent(string conversationId)
		{
			return context
				.ScheduledMessageEvents
				.Include(@event => @event.ScheduledMessage)
				.ThenInclude(message => message.Details)
				.Where(@event =>
					@event.State == ScheduledMessageEventState.Pending &&
					@event.ScheduledMessage.State == ScheduledMessageState.Active &&
					@event.ScheduledMessage.Details.ConversationId.Equals(conversationId, StringComparison.Ordinal))
				.OrderBy(@event => @event.NextOccurence)
				.FirstOrDefault();
		}

		private ScheduledMessage GetMessageById(string conversationId, Guid messageId)
		{
			return context
				.ScheduledMessages
				.Include(message => message.Details)
				.Include(message => message.Events)
				.FirstOrDefault(message =>
					message.Id == messageId &&
					message.State == ScheduledMessageState.Active &&
					message.Details.ConversationId.Equals(conversationId, StringComparison.Ordinal));
		}

		private bool IsRequestedCountValid(int count)
		{
			return count > 0 && count <= maxMessageCount;
		}

		private static string BuildResponseMessageText(ScheduledMessage message, IEnumerable<DateTime> nextOccurences, CultureInfo clientCulture)
		{
			StringBuilder stringBuilder = new StringBuilder();
			TimeSpan timeZoneOffset = message.Details.TimeZoneOffset.GetValueOrDefault();

			stringBuilder
				.AppendFormat("ID: '{0}'", message.Id)
				.Append(MessageUtils.NewLine)
				.AppendFormat("Message: '{0}'", message.Text)
				.Append(MessageUtils.NewLine);

			foreach (DateTime occurence in nextOccurences)
			{
				DateTime adjustedOccurence = occurence.Add(timeZoneOffset);

				stringBuilder
					.Append(MessageUtils.NewLine)
					.AppendFormat("Occurence: {0}", adjustedOccurence.ToString(clientCulture));
			}

			return stringBuilder.ToString().Trim();
		}

		private static CommandExecutionResult GetNoScheduledMessagesResult()
		{
			return CommandExecutionResult.Success("No scheduled events for this conversation");
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

		#endregion
	}
}
