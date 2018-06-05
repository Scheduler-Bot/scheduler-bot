using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchedulerBot.Business.Commands.Utils;
using SchedulerBot.Business.Entities;
using SchedulerBot.Business.Utils;
using SchedulerBot.Database.Core;
using SchedulerBot.Database.Entities;
using SchedulerBot.Database.Entities.Enums;
using SchedulerBot.Database.Interfaces;
using SchedulerBot.Infrastructure.Interfaces.Configuration;
using SchedulerBot.Infrastructure.Interfaces.Schedule;

namespace SchedulerBot.Business.Commands
{
	/// <summary>
	/// The command listing the next scheduled message occurrences for the current conversation.
	/// </summary>
	/// <seealso cref="BotCommand" />
	public class NextCommand : BotCommand
	{
		#region Private Fields

		private readonly int defaultMessageCount;
		private readonly int maxMessageCount;
		private readonly IScheduleParser scheduleParser;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="NextCommand" /> class.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="scheduleParser">The schedule parser.</param>
		/// <param name="configuration">The configuration.</param>
		/// <param name="unitOfWork">The unit of work.</param>
		/// <param name="logger">The logger.</param>
		public NextCommand(
			IScheduleParser scheduleParser,
			INextCommandConfiguration configuration,
			IUnitOfWork unitOfWork,
			ILogger<ListCommand> logger) : base("next", unitOfWork, logger)
		{
			this.scheduleParser = scheduleParser;

			defaultMessageCount = configuration.DefaultMessageCount;
			maxMessageCount = configuration.MaxMessageCount;
		}

		#endregion

		#region Overrides

		/// <inheritdoc />
		protected override async Task<CommandExecutionResult> ExecuteCoreAsync(Activity activity, string arguments)
		{
			CultureInfo clientCulture = CultureUtils.GetCultureInfoOrDefault(activity.Locale);

			CommandExecutionResult result = string.IsNullOrWhiteSpace(arguments)
				? await ExecuteWithNoArgumentsAsync(activity, clientCulture)
				: await ExecuteWithArguments(activity, arguments, clientCulture);

			return result;
		}

		#endregion

		#region Private Methods

		private async Task<CommandExecutionResult> ExecuteWithNoArgumentsAsync(Activity activity, CultureInfo clientCulture)
		{
			Logger.LogInformation("No arguments provided");

			ScheduledMessageEvent nextEvent = 
				await UnitOfWork.ScheduledMessageEvents.GetNextMessageEventAsync(activity.Conversation.Id);

			CommandExecutionResult executionResult = nextEvent != null
				? ExecuteWithMessageAndCount(nextEvent.ScheduledMessage, defaultMessageCount, clientCulture)
				: GetNoScheduledMessagesResult();

			return executionResult;
		}

		private async Task<CommandExecutionResult> ExecuteWithArguments(
			Activity activity,
			string arguments,
			CultureInfo clientCulture)
		{
			Logger.LogInformation("Provided arguments: {0}", arguments);

			CommandExecutionResult result;
			string[] splitArguments = ArgumentHelper.ParseArguments(arguments);
			string messageIdArgument = splitArguments.ElementAtOrDefault(0);

			if (Guid.TryParse(messageIdArgument, out Guid messageId))
			{
				ScheduledMessage scheduledMessage = await UnitOfWork.ScheduledMessages
					.GetActiveByIdAndConversationIdAsync(activity.Conversation.Id, messageId);

				if (scheduledMessage != null)
				{
					string countArgument = splitArguments.ElementAtOrDefault(1);

					if (countArgument != null)
					{
						if (int.TryParse(countArgument, NumberStyles.Integer, clientCulture, out int count))
						{
							result = ExecuteWithMessageAndCount(scheduledMessage, count, clientCulture);
						}
						else
						{
							Logger.LogError("Cannot parse count '{0}'", countArgument);
							result = CommandExecutionResult.Error($"Cannot retrieve the requested number of events from the argument '{countArgument}'");
						}
					}
					else
					{
						result = ExecuteWithMessage(scheduledMessage, clientCulture);
					}
				}
				else
				{
					Logger.LogWarning("No message with id '{0}'", messageIdArgument);
					result = GetNoScheduledMessagesResult();
				}
			}
			else
			{
				Logger.LogError("Cannot parse message id '{0}'", messageIdArgument);
				result = CommandExecutionResult.Error($"Cannot parse the message id '{messageIdArgument}'");
			}

			return result;
		}

		private CommandExecutionResult ExecuteWithMessage(ScheduledMessage message, CultureInfo clientCulture)
		{
			Logger.LogInformation("Executing with message id '{0}' default message count", message.Id);

			ScheduledMessageEvent nextEvent = message
				.Events
				.Where(@event => @event.State == ScheduledMessageEventState.Pending)
				.OrderBy(@event => @event.NextOccurrence)
				.FirstOrDefault();
			CommandExecutionResult executionResult = nextEvent != null
				? ExecuteWithMessageAndCount(nextEvent.ScheduledMessage, defaultMessageCount, clientCulture)
				: GetNoScheduledMessagesResult();

			return executionResult;
		}

		private CommandExecutionResult ExecuteWithMessageAndCount(ScheduledMessage message, int count, CultureInfo clientCulture)
		{
			Logger.LogInformation("Executing with message id '{0}' and count '{1}'", message.Id, count);

			CommandExecutionResult executionResult;

			if (IsRequestedCountValid(count))
			{
				bool hasPendingEvents = message
					.Events
					.Any(@event => @event.State == ScheduledMessageEventState.Pending);

				if (hasPendingEvents)
				{
					ISchedule schedule = scheduleParser.Parse(message.Schedule, message.Details.TimeZoneOffset);
					IEnumerable<DateTime> messageOccurrences = schedule
						.GetNextOccurrences(DateTime.UtcNow, DateTime.MaxValue)
						.Take(count);

					executionResult = BuildResponseMessageText(message, messageOccurrences, clientCulture);
				}
				else
				{
					Logger.LogWarning("Message with id '{0}' does not have any scheduled events", message.Id);
					executionResult = GetNoScheduledMessagesResult();
				}
			}
			else
			{
				Logger.LogError("Requested count '{0}' is invalid", count);
				executionResult = CommandExecutionResult.Error($"The requested message count must be between 1 and {maxMessageCount}");
			}

			return executionResult;
		}

		private bool IsRequestedCountValid(int count)
		{
			return count > 0 && count <= maxMessageCount;
		}

		private static string BuildResponseMessageText(ScheduledMessage message, IEnumerable<DateTime> nextOccurrences, CultureInfo clientCulture)
		{
			StringBuilder stringBuilder = new StringBuilder();
			TimeSpan timeZoneOffset = message.Details.TimeZoneOffset.GetValueOrDefault();
			string newLine = MessageUtils.NewLine;

			stringBuilder
				.AppendFormat("ID: '{0}'", message.Id)
				.Append(newLine)
				.AppendFormat("Message: '{0}'", message.Text)
				.Append(newLine)
				.Append("Occurrences:");

			foreach (DateTime occurrence in nextOccurrences)
			{
				DateTime adjustedOccurrence = occurrence.Add(timeZoneOffset);

				stringBuilder
					.Append(newLine)
					.Append(adjustedOccurrence.ToString(clientCulture));
			}

			return stringBuilder.ToString().Trim();
		}

		private static CommandExecutionResult GetNoScheduledMessagesResult()
		{
			return CommandExecutionResult.Success("No scheduled events for this conversation");
		}

		#endregion
	}
}
