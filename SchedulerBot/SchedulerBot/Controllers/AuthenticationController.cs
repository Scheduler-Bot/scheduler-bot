using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchedulerBot.Business.Entities;
using SchedulerBot.Business.Interfaces.Services;
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

		private readonly IManageConversationLinkService manageConversationLinkService;
		private readonly IJwtTokenGenerator tokenGenerator;
		private readonly ILogger<ManageConversationController> logger;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthenticationController" /> class.
		/// </summary>
		/// <param name="manageConversationLinkService">The manage conversation link service.</param>
		/// <param name="tokenGenerator">The JWT token generator.</param>
		/// <param name="logger">The logger.</param>
		public AuthenticationController(
			IManageConversationLinkService manageConversationLinkService,
			IJwtTokenGenerator tokenGenerator,
			ILogger<ManageConversationController> logger)
		{
			this.manageConversationLinkService = manageConversationLinkService;
			this.tokenGenerator = tokenGenerator;
			this.logger = logger;
		}

		#endregion

		#region Actions

		/// <summary>
		/// Handles user requests for temporary authentication to manage conversations.
		/// </summary>
		/// <param name="request">The manage conversation authentication request.</param>
		/// <returns>The action result.</returns>
		[HttpPost("signin")]
		public async Task<IActionResult> SignIn([FromBody] ManageConversationAuthenticationRequest request)
		{
			string manageId = request.ManageId;

			logger.LogInformation("Attempting to gather managing information for the id '{0}'", manageId);

			IActionResult actionResult;
			CommandExecutionResult executionResult = await manageConversationLinkService.ValidateAndMarkVisitedAsync(manageId);

			if (executionResult.IsSuccess)
			{
				logger.LogInformation("Generating temporary access token for conversation id '{0}'", manageId);

				string token = tokenGenerator.GenerateToken($"temp-manage-user-{manageId}");
				TokenResponse tokenResponse = new TokenResponse(token);

				logger.LogInformation("Generated temporary access token for conversation id '{0}'", manageId);

				actionResult = Ok(tokenResponse);
			}
			else
			{
				actionResult = NotFound();
			}

			return actionResult;
		}

		#endregion
	}
}
