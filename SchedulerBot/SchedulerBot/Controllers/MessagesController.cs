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

		public MessagesController(IConfiguration configuration)
		{
			this.configuration = configuration;
		}

		[Authorize(Roles = "Bot")]
		[HttpPost]
		public async Task<OkResult> Post([FromBody] Activity activity)
		{
			if (activity.Type == ActivityTypes.Message)
			{
				//MicrosoftAppCredentials.TrustServiceUrl(activity.ServiceUrl);
				MicrosoftAppCredentials appCredentials = new MicrosoftAppCredentials(configuration[MicrosoftAppCredentials.MicrosoftAppIdKey], configuration[MicrosoftAppCredentials.MicrosoftAppPasswordKey]);
				ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl), appCredentials);

				// return our reply to the user
				Activity reply = activity.CreateReply("HelloWorld");

				await connector.Conversations.ReplyToActivityAsync(reply);
			}
			else
			{
				//HandleSystemMessage(activity);
			}

			return Ok();
		}
	}
}