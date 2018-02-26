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
		private readonly SchedulerBotContext context;
		private readonly IScheduleParser scheduleParser;
		private readonly ILogger<ListCommand> logger;

		public NextCommand(
			SchedulerBotContext context,
			IScheduleParser scheduleParser,
			ILogger<ListCommand> logger)
		{
			this.context = context;
			this.scheduleParser = scheduleParser;
			this.logger = logger;

			Name = "next";
		}

		public string Name { get; }

		public Task<CommandExecutionResult> ExecuteAsync(Activity activity, string arguments)
		{
			logger.LogInformation("Executing '{0}' command", Name);

			CommandExecutionResult result = null;
			ScheduledMessage nextScheduledMessage = null;
			List<DateTime> nextOccurences = new List<DateTime>();
			CultureInfo clientCulture = GetCultureInfoOrDefault(activity.Locale);

			if (string.IsNullOrWhiteSpace(arguments))
			{
				ScheduledMessageEvent nextEvent = GetNextMessageEvent(activity.Conversation.Id);

				if (nextEvent != null)
				{
					nextScheduledMessage = nextEvent.ScheduledMessage;
					nextOccurences.Add(nextEvent.NextOccurence);
				}
				else
				{
					result = CommandExecutionResult.Success("No scheduled events for this conversation");
				}
			}
			else
			{
				string[] splitArguments = ArgumentHelper.ParseArguments(arguments);
				string stringMessageId = splitArguments.ElementAtOrDefault(0);

				if (Guid.TryParse(stringMessageId, out Guid messageId))
				{
					nextScheduledMessage = GetNextMessageById(activity.Conversation.Id, messageId);

					if (nextScheduledMessage != null)
					{
						string stringCount = splitArguments.ElementAtOrDefault(1);

						if (stringCount != null)
						{
							if (int.TryParse(stringCount, NumberStyles.Integer, clientCulture, out int count))
							{
								bool scheduledMessageExists = nextScheduledMessage
									.Events
									.Any(@event => @event.State == ScheduledMessageEventState.Pending);

								if (scheduledMessageExists)
								{
									ISchedule schedule = scheduleParser.Parse(nextScheduledMessage.Schedule, nextScheduledMessage.Details.TimeZoneOffset);
									IEnumerable<DateTime> messageOccurences = schedule
										.GetNextOccurences(DateTime.UtcNow, DateTime.MaxValue)
										.Take(count);

									nextOccurences.AddRange(messageOccurences);
								}
								else
								{
									result = CommandExecutionResult.Success("No scheduled events for this conversation");
								}
							}
							else
							{
								result = CommandExecutionResult.Error($"Cannot retrieve the requested number of events from the argument '{stringCount}'");
							}
						}
						else
						{
							ScheduledMessageEvent nextEvent = nextScheduledMessage
								.Events
								.Where(@event => @event.State == ScheduledMessageEventState.Pending)
								.OrderBy(@event => @event.NextOccurence)
								.FirstOrDefault();

							if (nextEvent != null)
							{
								nextOccurences.Add(nextEvent.NextOccurence);
							}
							else
							{
								result = CommandExecutionResult.Success("No scheduled events for this conversation");
							}
						}
					}
					else
					{
						result = CommandExecutionResult.Success("No scheduled events for this conversation");
					}
				}
				else
				{
					result = CommandExecutionResult.Error($"Cannot parse the message id '{stringMessageId}'");
				}
			}

			if (nextOccurences.Count > 0 && nextScheduledMessage != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				TimeSpan timeZoneOffset = nextScheduledMessage.Details.TimeZoneOffset.GetValueOrDefault();

				stringBuilder
					.AppendFormat("ID: '{0}'", nextScheduledMessage.Id)
					.Append(MessageUtils.NewLine)
					.AppendFormat("Message: '{0}'", nextScheduledMessage.Text)
					.Append(MessageUtils.NewLine);

				foreach (DateTime occurence in nextOccurences)
				{
					DateTime adjustedOccurence = occurence.Add(timeZoneOffset);

					stringBuilder
						.AppendFormat("Occurence: {0}", adjustedOccurence.ToString(clientCulture))
						.Append(MessageUtils.NewLine);
				}

				result = CommandExecutionResult.Success(stringBuilder.ToString().Trim());
			}

			return Task.FromResult(result ?? CommandExecutionResult.Error("Unexpected error"));
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

		private ScheduledMessage GetNextMessageById(string conversationId, Guid messageId)
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
	}
}
