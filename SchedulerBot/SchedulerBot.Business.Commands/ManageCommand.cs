using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchedulerBot.Business.Entities;
using SchedulerBot.Database.Core;
using SchedulerBot.Database.Entities;
using SchedulerBot.Infrastructure.Interfaces.Application;
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
		#region Private Fields

		private readonly SchedulerBotContext context;
		private readonly IWebUtility webUtility;
		private readonly IApplicationContext applicationContext;
		private readonly TimeSpan linkExpirationPeriod;
		private readonly int linkIdLength;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ManageCommand"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="webUtility">The web utility.</param>
		/// <param name="applicationContext">The application context.</param>
		/// <param name="configuration">The configuration.</param>
		/// <param name="logger">The logger.</param>
		public ManageCommand(
			SchedulerBotContext context,
			IWebUtility webUtility,
			IApplicationContext applicationContext,
			IConfiguration configuration,
			ILogger<ManageCommand> logger) : base("manage", logger)
		{
			this.context = context;
			this.webUtility = webUtility;
			this.applicationContext = applicationContext;

			linkExpirationPeriod = TimeSpan.Parse(configuration["Commands:Manage:LinkExpirationPeriod"], CultureInfo.InvariantCulture);
			linkIdLength = int.Parse(configuration["Commands:Manage:LinkIdLength"], CultureInfo.InvariantCulture);
		}

		#endregion

		#region Overrides

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

		#endregion

		#region Private Methods

		private ManageConversationLink CreateManageConversationLink(Activity activity)
		{
			string linkText = webUtility.GenerateRandomUrlCompatibleString(linkIdLength);
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

		#endregion
	}
}
