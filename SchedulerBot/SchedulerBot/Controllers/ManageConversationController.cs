using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchedulerBot.Authentication;
using SchedulerBot.Database.Core;
using SchedulerBot.Database.Entities;
using SchedulerBot.Database.Entities.Enums;
using SchedulerBot.Infrastructure.Interfaces.Schedule;
using SchedulerBot.Models;

namespace SchedulerBot.Controllers
{
	/// <summary>
	/// The API controller for managing conversations.
	/// </summary>
	/// <seealso cref="Controller" />
	[Route("api/manage")]
	public class ManageConversationController : Controller
	{
		#region Constants

		private const int NextOccurrenceCount = 5;

		#endregion

		#region Private Fields

		private readonly SchedulerBotContext context;
		private readonly IScheduleParser scheduleParser;
		private readonly ILogger<ManageConversationController> logger;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ManageConversationController"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="scheduleParser">The schedule parser.</param>
		/// <param name="logger">The logger.</param>
		public ManageConversationController(
			SchedulerBotContext context,
			IScheduleParser scheduleParser,
			ILogger<ManageConversationController> logger)
		{
			this.context = context;
			this.scheduleParser = scheduleParser;
			this.logger = logger;
		}

		#endregion

		#region Actions

		/// <summary>
		/// Handles HTTP GET requests returning the information about a specific conversation.
		/// </summary>
		/// <param name="manageId">The temporary identifier assigned to a particular conversation.</param>
		/// <returns>The result of the method processing.</returns>
		[HttpGet("{manageId}")]
		[Authorize(AuthenticationSchemes = ManageConversationAuthenticationConfiguration.AuthenticationSchemeName)]
		public async Task<IActionResult> Get(string manageId)
		{
			logger.LogInformation("Attempting to gather managing information for the id '{0}'", manageId);

			IActionResult actionResult;
			ManageConversationLink manageLink = await context
				.ManageConversationLinks
				.FirstOrDefaultAsync(link => link.Text.ToUpper(CultureInfo.InvariantCulture) == manageId.ToUpper(CultureInfo.InvariantCulture));

			if (manageLink != null)
			{
				logger.LogInformation("Returning managing information for the id '{0}'", manageId);
				actionResult = Ok(GetConversationScheduledMessageModels(manageLink).ToList());
			}
			else
			{
				logger.LogInformation("No managing information has been found for the id '{0}'", manageId);
				actionResult = NotFound();
			}

			return actionResult;
		}

		#endregion

		#region Private Methods

		private IEnumerable<ScheduledMessageModel> GetConversationScheduledMessageModels(ManageConversationLink manageLink)
		{
			return context
				.ScheduledMessageDetails
				.Include(messageDetails => messageDetails.ScheduledMessage)
				.ThenInclude(message => message.Events)
				.Where(messageDetails =>
					messageDetails.ChannelId.ToUpper(CultureInfo.InvariantCulture) == manageLink.ChannelId.ToUpper(CultureInfo.InvariantCulture) &&
					messageDetails.ConversationId.ToUpper(CultureInfo.InvariantCulture) == manageLink.ConversationId.ToUpper(CultureInfo.InvariantCulture))
				.AsEnumerable()
				.Select(CreateScheduledMessageModel);
		}

		private ScheduledMessageModel CreateScheduledMessageModel(ScheduledMessageDetails scheduledMessageDetails)
		{
			ScheduledMessage scheduledMessage = scheduledMessageDetails.ScheduledMessage;

			return new ScheduledMessageModel
			{
				Id = scheduledMessageDetails.ScheduledMessageId,
				Text = scheduledMessage.Text,
				State = scheduledMessage.State,
				Locale = scheduledMessageDetails.Locale,
				NextOccurrences = GetNextOccurrences(scheduledMessageDetails, NextOccurrenceCount)
			};
		}

		private IList<DateTime> GetNextOccurrences(ScheduledMessageDetails scheduledMessageDetails, int count)
		{
			IList<DateTime> nextOccurrences = null;
			ScheduledMessage scheduledMessage = scheduledMessageDetails.ScheduledMessage;
			bool hasPendingEvents = scheduledMessage
				.Events
				.Any(@event => @event.State == ScheduledMessageEventState.Pending);

			if (hasPendingEvents)
			{
				ISchedule schedule = scheduleParser.Parse(scheduledMessage.Schedule, scheduledMessageDetails.TimeZoneOffset);
				nextOccurrences = schedule
					.GetNextOccurrences(DateTime.UtcNow, DateTime.MaxValue)
					.Take(count)
					.ToList();
			}

			return nextOccurrences ?? new DateTime[0];
		}

		#endregion
	}
}
