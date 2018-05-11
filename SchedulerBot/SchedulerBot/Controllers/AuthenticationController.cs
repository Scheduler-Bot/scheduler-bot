using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SchedulerBot.Database.Core;
using SchedulerBot.Database.Entities;
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
		private readonly IConfiguration configuration;
		private readonly ILogger<ManageConversationController> logger;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthenticationController"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		/// /// <param name="configuration">The configuration.</param>
		/// <param name="logger">The logger.</param>
		public AuthenticationController(
			SchedulerBotContext context,
			IConfiguration configuration,
			ILogger<ManageConversationController> logger)
		{
			this.context = context;
			this.configuration = configuration;
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

				SecurityTokenDescriptor descriptor = CreateTokenDescriptor(manageId);
				JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
				SecurityToken token = tokenHandler.CreateToken(descriptor);
				string tokenString = tokenHandler.WriteToken(token);
				TokenResponse tokenResponse = new TokenResponse(tokenString);

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

		#region Private Methods

		private SecurityTokenDescriptor CreateTokenDescriptor(string manageId)
		{
			string base64SigningKey = configuration["Secrets:Authentication[0]:SigningKey"];
			string issuer = configuration["Secrets:Authentication[0]:Issuer"];
			string audience = configuration["Secrets:Authentication[0]:Audience"];
			TimeSpan expirationPeriod = TimeSpan.Parse(configuration["Secrets:Authentication[0]:ExpirationPeriod"], CultureInfo.InvariantCulture);

			SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Convert.FromBase64String(base64SigningKey));
			SigningCredentials credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
			DateTime currentDateTime = DateTime.UtcNow;
			SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = CreateTempManageIdentity(manageId),
				Audience = audience,
				Issuer = issuer,
				IssuedAt = currentDateTime,
				NotBefore = currentDateTime,
				Expires = currentDateTime + expirationPeriod,
				SigningCredentials = credentials
			};

			return tokenDescriptor;
		}

		private static ClaimsIdentity CreateTempManageIdentity(string manageId)
		{
			return new ClaimsIdentity(new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, $"temp-manage-user-{manageId}"),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N"))
			});
		}

		#endregion
	}
}
