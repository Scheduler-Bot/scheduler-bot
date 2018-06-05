using System;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using SchedulerBot.Business.Entities;
using SchedulerBot.Database.Entities;
using SchedulerBot.Database.Interfaces;
using SchedulerBot.Infrastructure.Interfaces.Application;
using SchedulerBot.Infrastructure.Interfaces.Configuration;
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

		private readonly IWebUtility webUtility;
		private readonly IApplicationContext applicationContext;
		private readonly TimeSpan linkExpirationPeriod;
		private readonly int linkIdLength;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ManageCommand" /> class.
		/// </summary>
		/// <param name="webUtility">The web utility.</param>
		/// <param name="applicationContext">The application context.</param>
		/// <param name="configuration">The configuration.</param>
		/// <param name="unitOfWork">The unit of work.</param>
		/// <param name="logger">The logger.</param>
		public ManageCommand(
			IWebUtility webUtility,
			IApplicationContext applicationContext,
			IManageCommandConfiguration configuration,
			IUnitOfWork unitOfWork,
			ILogger<ManageCommand> logger) : base("manage", unitOfWork, logger)
		{
			this.webUtility = webUtility;
			this.applicationContext = applicationContext;

			linkExpirationPeriod = configuration.LinkExpirationPeriod;
			linkIdLength = configuration.LinkIdLength;
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

			string linkId = webUtility.GenerateRandomUrlCompatibleString(linkIdLength);
			ManageConversationLink manageConversationLink = CreateManageConversationLink(activity, linkId);

			await UnitOfWork.ManageConversationLinks.AddAsync(manageConversationLink);
			await UnitOfWork.SaveChangesAsync();

			Logger.LogInformation(
				"Created the manage link '{0}' for channel '{1}' and conversation '{2}'",
				manageConversationLink.Text,
				activity.ChannelId,
				activity.Conversation.Id);

			string manageUrl = webUtility.GenerateUrl(
				applicationContext.Protocol,
				applicationContext.Host,
				"manage",
				linkId);

			return CommandExecutionResult.Success(manageUrl);
		}

		#endregion

		#region Private Methods

		private ManageConversationLink CreateManageConversationLink(Activity activity, string linkId)
		{
			DateTime linkCreationTime = DateTime.UtcNow;
			DateTime linkExpirationTime = linkCreationTime + linkExpirationPeriod;

			return new ManageConversationLink
			{
				ChannelId = activity.ChannelId,
				ConversationId = activity.Conversation.Id,
				Text = linkId,
				CreatedOn = linkCreationTime,
				ExpiresOn = linkExpirationTime
			};
		}

		#endregion
	}
}
