using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using SchedulerBot.Business.Entities;
using SchedulerBot.Business.Interfaces;
using SchedulerBot.Infrastructure.Interfaces.BotConnector;

namespace SchedulerBot.Controllers
{
	[Route("api/[controller]")]
	public class MessagesController : Controller
	{
		private readonly ICommandRequestParser commandRequestParser;
		private readonly ICommandSelector commandSelector;
		private readonly IMessageProcessor messageProcessor;
		private readonly ILogger<MessagesController> logger;

		public MessagesController(
			ICommandRequestParser commandRequestParser,
			ICommandSelector commandSelector,
			IMessageProcessor messageProcessor,
			ILogger<MessagesController> logger)
		{
			this.commandRequestParser = commandRequestParser;
			this.commandSelector = commandSelector;
			this.messageProcessor = messageProcessor;
			this.logger = logger;
		}

		[HttpGet]
		public IActionResult Get()
		{
			return Ok("MessagesController");
		}

		[Authorize(Roles = "Bot")]
		[HttpPost]
		public async Task<OkResult> Post([FromBody] Activity activity)
		{
			if (activity.Type == ActivityTypes.Message)
			{
				DecodeActivityText(activity);

				string replyText = null;
				CommandRequestParseResult parsedCommandRequest = commandRequestParser.Parse(activity);

				if (parsedCommandRequest != null)
				{
					IBotCommand command = commandSelector.GetCommand(parsedCommandRequest.Name);

					if (command != null)
					{
						replyText = (await command.ExecuteAsync(activity, parsedCommandRequest.Arguments)).Message;
					}
				}

				if (replyText == null)
				{
					replyText = "Sorry, I don't understand you :(";
				}

				logger.LogInformation("Replying with '{0}'", replyText);

				await messageProcessor.ReplyAsync(activity, replyText, CancellationToken.None);
			}

			return Ok();
		}

		private void DecodeActivityText(Activity activity)
		{
			logger.LogInformation("Received the message with the following text: '{0}'", activity.Text);
			activity.Text = WebUtility.HtmlDecode(activity.Text);
			logger.LogInformation("Decoded the text to '{0}'", activity.Text);
		}
	}
}
