using System.Text.RegularExpressions;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using SchedulerBot.Business.Entities;
using SchedulerBot.Business.Interfaces.Commands;

namespace SchedulerBot.Business.Commands.Utils
{
	/// <summary>
	/// The parser for converting a user request into the information about the requested command.
	/// </summary>
	/// <seealso cref="ICommandRequestParser" />
	public class CommandRequestParser : ICommandRequestParser
	{
		#region Constants

		private const string NameGroup = "name";
		private const string ArgumentsGroup = "arguments";
		private const string Pattern = @"^\s*(?'" + NameGroup + @"'[^\s]+)\s*(?'" + ArgumentsGroup + @"'.*)\s*$";

		#endregion

		#region Private Fields

		private static readonly Regex CommandRegex = new Regex(Pattern);
		private readonly ILogger<CommandRequestParser> logger;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="CommandRequestParser"/> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		public CommandRequestParser(ILogger<CommandRequestParser> logger)
		{
			this.logger = logger;
		}

		#endregion

		#region ICommandRequestParser Implementation

		/// <inheritdoc />
		public ParsedCommandRequest Parse(Activity activity)
		{
			string commandRequestText = RemoveRecipientMention(activity);

			logger.LogInformation("Parsing command request '{0}'", commandRequestText);

			ParsedCommandRequest parseResult = null;
			Match match = CommandRegex.Match(commandRequestText);

			if (match.Success)
			{
				string name = match.Groups[NameGroup].Value;
				string arguments = match.Groups[ArgumentsGroup]?.Value ?? string.Empty;

				parseResult = new ParsedCommandRequest(name, arguments);
				logger.LogInformation("Command request parsed to command '{0}' with arguments '{1}'", name, arguments);
			}

			if (parseResult == null)
			{
				logger.LogWarning("Command request '{0}' cannot be parsed", commandRequestText);
			}

			return parseResult;
		}

		#endregion

		#region Private Methods

		// This is a workaround for removing entries like '@scheduler-bot' from a message.
		// The RemoveRecipientMention method provided by Microsoft.Bot.Schema.ActivityExtensions
		// has a known issue and doesn't work for Skype mentions:
		// https://github.com/Microsoft/BotBuilder/issues/3067
		private static string RemoveRecipientMention(Activity activity)
		{
			string mentionText = "@" + activity.Recipient.Name;

			return Regex.Replace(activity.Text, mentionText, string.Empty, RegexOptions.IgnoreCase);
		}

		#endregion
	}
}
