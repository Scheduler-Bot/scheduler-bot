﻿using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Rest;
using SchedulerBot.Business.Entities;
using SchedulerBot.Business.Interfaces;

namespace SchedulerBot.Controllers
{
	[Route("api/[controller]")]
	public class MessagesController : Controller
	{
		private readonly ServiceClientCredentials credentials;
		private readonly ICommandRequestParser commandRequestParser;
		private readonly ICommandSelector commandSelector;
		private readonly ILogger<MessagesController> logger;

		public MessagesController(
			ServiceClientCredentials credentials,
			ICommandRequestParser commandRequestParser,
			ICommandSelector commandSelector,
			ILogger<MessagesController> logger)
		{
			this.credentials = credentials;
			this.commandRequestParser = commandRequestParser;
			this.commandSelector = commandSelector;
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

				await ReplyAsync(activity, replyText);
			}

			return Ok();
		}

		private Task<ResourceResponse> ReplyAsync(Activity activity, string replyText)
		{
			Activity reply = activity.CreateReply(replyText, activity.Locale);

			using (ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl), credentials))
			{
				return connector.Conversations.ReplyToActivityAsync(reply);
			}
		}

		private void DecodeActivityText(Activity activity)
		{
			logger.LogInformation("Received the message with the following text: '{0}'", activity.Text);
			activity.Text = WebUtility.HtmlDecode(activity.Text);
			logger.LogInformation("Decoded the text to '{0}'", activity.Text);
		}
	}
}