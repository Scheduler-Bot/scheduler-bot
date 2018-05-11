using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchedulerBot.Database.Core;
using SchedulerBot.Database.Entities;
using SchedulerBot.Infrastructure.Interfaces.Authentication;
using SchedulerBot.Models;

namespace SchedulerBot.Controllers
{
	/// <summary>
	/// The API controller used to authenticate users.
	/// </summary>
	/// <seealso cref="Controller" />
	[Produces("application/json")]
	[Route("api/auth")]
	public class AuthenticationController : Controller
	{
		#region Private Fields

		private readonly SchedulerBotContext context;
		private readonly IJwtTokenGenerator tokenGenerator;
		private readonly ILogger<ManageConversationController> logger;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthenticationController"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="tokenGenerator">The JWT token generator.</param>
		/// <param name="logger">The logger.</param>
		public AuthenticationController(
			SchedulerBotContext context,
			IJwtTokenGenerator tokenGenerator,
			ILogger<ManageConversationController> logger)
		{
			this.context = context;
			this.tokenGenerator = tokenGenerator;
			this.logger = logger;
		}

		#endregion

		#region Actions

		/// <summary>
		/// Handles user requests for temporary authentication to manage conversations.
		/// </summary>
		/// <param name="manageId">The manage conversation identifier.</param>
		/// <returns>The action result.</returns>
		[HttpPost("signin/{manageId}")]
		public async Task<IActionResult> SignIn(string manageId)
		{
			logger.LogInformation("Attempting to gather managing information for the id '{0}'", manageId);

			IActionResult actionResult;
			ManageConversationLink manageLink = await context
				.ManageConversationLinks
				.FirstOrDefaultAsync(link => link.Text == manageId);

			if (manageLink != null && !manageLink.IsVisited && manageLink.ExpiresOn > DateTime.UtcNow)
			{
				logger.LogInformation("Marking manage conversation id '{0}' as visited", manageId);

				manageLink.IsVisited = true;

				await context.SaveChangesAsync();

				logger.LogInformation("Generating temporary access token for conversation id '{0}'", manageId);

				string token = tokenGenerator.GenerateToken($"temp-manage-user-{manageId}");
				TokenResponse tokenResponse = new TokenResponse(token);

				logger.LogInformation("Generated temporary access token for conversation id '{0}'", manageId);

				actionResult = Ok(tokenResponse);
			}
			else
			{
				logger.LogInformation("No managing information has been found for the id '{0}'. Either the link does not exist or is has expired", manageId);
				actionResult = NotFound();
			}

			return actionResult;
		}

		#endregion
	}
}
