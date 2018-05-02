using System;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchedulerBot.Business.Entities;
using SchedulerBot.Database.Core;
using SchedulerBot.Database.Entities;
using SchedulerBot.Infrastructure.Interfaces.Utils;

namespace SchedulerBot.Business.Commands
{
	/// <summary>
	/// The command allowing for a user to get a link to a temporary page
	/// where they can manage the events for the current conversation.
	/// </summary>
	/// <seealso cref="BotCommand" />
	public class ManageCommand : BotCommand
	{
		private readonly SchedulerBotContext context;
		private readonly IRandomByteGenerator randomByteGenerator;
		private readonly TimeSpan linkExpirationPeriod;
		private readonly int linkIdLength;

		/// <summary>
		/// Initializes a new instance of the <see cref="ManageCommand"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="randomByteGenerator">The random byte generator.</param>
		/// <param name="configuration">The configuration.</param>
		/// <param name="logger">The logger.</param>
		public ManageCommand(
			SchedulerBotContext context,
			IRandomByteGenerator randomByteGenerator,
			IConfiguration configuration,
			ILogger<ManageCommand> logger) : base("manage", logger)
		{
			this.context = context;
			this.randomByteGenerator = randomByteGenerator;

			linkExpirationPeriod = TimeSpan.Parse(configuration["Commands:Manage:LinkExpirationPeriod"], CultureInfo.InvariantCulture);
			linkIdLength = int.Parse(configuration["Commands:Manage:LinkIdLength"], CultureInfo.InvariantCulture);
		}

		/// <inheritdoc />
		protected override async Task<CommandExecutionResult> ExecuteCoreAsync(Activity activity, string arguments)
		{
			Logger.LogInformation(
				"Creating a manage link for channel '{0}' and conversation '{1}'",
				activity.ChannelId,
				activity.Conversation.Id);

			ManageConversationLink manageConversationLink = CreateManageConversationLink(activity);

			await context.ManageConversationLinks.AddAsync(manageConversationLink);
			await context.SaveChangesAsync();

			Logger.LogInformation(
				"Created the manage link '{0}' for channel '{1}' and conversation '{2}'",
				manageConversationLink.Text,
				activity.ChannelId,
				activity.Conversation.Id);

			return CommandExecutionResult.Success(manageConversationLink.Text);
		}

		private ManageConversationLink CreateManageConversationLink(Activity activity)
		{
			string linkText = GenerateRandomString(linkIdLength);
			DateTime linkCreationTime = DateTime.UtcNow;
			DateTime linkExpirationTime = linkCreationTime + linkExpirationPeriod;

			return new ManageConversationLink
			{
				ChannelId = activity.ChannelId,
				ConversationId = activity.Conversation.Id,
				Text = linkText,
				CreatedOn = linkCreationTime,
				ExpiresOn = linkExpirationTime
			};
		}

		private string GenerateRandomString(int length)
		{
			string randomBase64String = GetRandomBase64String(length);
			string randomUrlEncodedString = UrlEncodeBase64String(randomBase64String, length);

			return randomUrlEncodedString;
		}

		private string GetRandomBase64String(int minLength)
		{
			byte[] randomBytes = randomByteGenerator.Generate(minLength);
			string randomBase64String = Convert.ToBase64String(randomBytes);

			return randomBase64String;
		}

		private static string UrlEncodeBase64String(string base64String, int length)
		{
			// Get rid of '=' chars at the end
			while (length > 0 && base64String[length - 1] == '=')
			{
				--length;
			}

			StringBuilder stringBuilder = new StringBuilder();

			for (int i = 0; i < length; i++)
			{
				char currentCharacter = base64String[i];
				char characterToAppend;

				switch (currentCharacter)
				{
					// '+' is not safe in url, so replace it with '-'
					case '+':
						characterToAppend = '-';
						break;
					// '/' is not safe in url, so replace it with '_'
					case '/':
						characterToAppend = '_';
						break;
					default:
						characterToAppend = currentCharacter;
						break;
				}

				stringBuilder.Append(characterToAppend);
			}

			return stringBuilder.ToString();
		}
	}
}
