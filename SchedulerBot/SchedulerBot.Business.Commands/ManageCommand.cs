using System;
using System.Globalization;
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
			string channelId = activity.ChannelId;
			string conversationId = activity.Conversation.Id;
			byte[] randomBytes = randomByteGenerator.Generate(64);
			string linkText = Convert.ToBase64String(randomBytes);
			DateTime linkCreationTime = DateTime.UtcNow;
			DateTime linkExpirationTime = linkCreationTime + linkExpirationPeriod;

			return new ManageConversationLink
			{
				ChannelId = channelId,
				ConversationId = conversationId,
				Text = linkText,
				CreatedOn = linkCreationTime,
				ExpiresOn = linkExpirationTime
			};
		}
	}
}
