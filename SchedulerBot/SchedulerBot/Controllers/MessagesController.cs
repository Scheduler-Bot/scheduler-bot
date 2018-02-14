using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;

namespace SchedulerBot.Controllers
{
	[Route("api/[controller]")]
	public class MessagesController : Controller
	{
		private readonly IConfiguration configuration;
		private readonly ICredentialProvider credentialProvider;

		public MessagesController(IConfiguration configuration, ICredentialProvider credentialProvider)
		{
			this.configuration = configuration;
			this.credentialProvider = credentialProvider;
		}

		[Authorize(Roles = "Bot")]
		[HttpPost]
		public async Task<OkResult> Post([FromBody] Activity activity)
		{
			if (activity.Type == ActivityTypes.Message)
			{
				//MicrosoftAppCredentials.TrustServiceUrl(activity.ServiceUrl);
				string appId = configuration[MicrosoftAppCredentials.MicrosoftAppIdKey];
				string appPassword = configuration[MicrosoftAppCredentials.MicrosoftAppPasswordKey];
				MicrosoftAppCredentials appCredentials = new MicrosoftAppCredentials(appId, appPassword);
				//var appCredentials = new MicrosoftAppCredentials();

				ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl), appCredentials);

				// return our reply to the user
				Activity reply = activity.CreateReply("Hello World");

				await connector.Conversations.ReplyToActivityAsync(reply);
			}
			else
			{
				//HandleSystemMessage(activity);
			}

			return Ok();
		}

		[HttpGet]
		public IActionResult Get()
		{
			return Ok("MessagesController");
		}
	}
}