using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchedulerBot.Authentication;
using SchedulerBot.Business.Interfaces.Services;
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

		private readonly IManageConversationLinkService manageConversationLinkService;
		private readonly IScheduledMessageDetailsService scheduledMessageDetailsService;

		private readonly IScheduleParser scheduleParser;
		private readonly ILogger<ManageConversationController> logger;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ManageConversationController" /> class.
		/// </summary>
		/// <param name="manageConversationLinkService">The manage conversation link service.</param>
		/// <param name="scheduledMessageDetailsService">The scheduled message details service.</param>
		/// <param name="scheduleParser">The schedule parser.</param>
		/// <param name="logger">The logger.</param>
		public ManageConversationController(
			IManageConversationLinkService manageConversationLinkService,
			IScheduledMessageDetailsService scheduledMessageDetailsService,
			IScheduleParser scheduleParser,
			ILogger<ManageConversationController> logger)
		{
			this.manageConversationLinkService = manageConversationLinkService;
			this.scheduledMessageDetailsService = scheduledMessageDetailsService;
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
			ManageConversationLink manageLink = await manageConversationLinkService.GetByTextAsync(manageId);

			if (manageLink != null)
			{
				logger.LogInformation("Returning managing information for the id '{0}'", manageId);
				actionResult = Ok(GetConversationScheduledMessageModelsAsync(manageLink));
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

		private async Task<IList<ScheduledMessageModel>> GetConversationScheduledMessageModelsAsync(ManageConversationLink manageLink)
		{
			IList<ScheduledMessageDetails> scheduledMessageDetails =
				await scheduledMessageDetailsService.GetByManageConversationLinkAsync(manageLink);

			List<ScheduledMessageModel> scheduledMessageModels
				= scheduledMessageDetails.Select(CreateScheduledMessageModel).ToList();

			return scheduledMessageModels;
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
