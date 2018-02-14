using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using SchedulerBot.Database.Core;
using SchedulerBot.Database.Entities;
using SchedulerBot.Infrastructure.Interfaces;

namespace SchedulerBot.Controllers
{
	[Route("api/[controller]")]
	public class MessagesController : Controller
	{
		private readonly SchedulerBotContext context;
		private readonly IConfiguration configuration;
		private readonly IScheduleParser scheduleParser;

		public MessagesController(SchedulerBotContext context, IConfiguration configuration, IScheduleParser scheduleParser)
		{
			this.context = context;
			this.configuration = configuration;
			this.scheduleParser = scheduleParser;
		}

		[Authorize(Roles = "Bot")]
		[HttpPost]
		public async Task<OkResult> Post([FromBody] Activity activity)
		{
			if (activity.Type == ActivityTypes.Message)
			{
				string replyText;
				string textSchedule = activity.Text;

				if (scheduleParser.TryParse(textSchedule, DateTime.UtcNow, out ISchedule schedule))
				{
					await AddScheduledMessageAsync(activity, schedule);

					replyText = $"Created an event with the following schedule: \"{textSchedule}\"";
				}
				else
				{
					replyText = $"Cannot recognize schedule \"{textSchedule}\"";
				}

				await ReplyAsync(activity, replyText);
			}

			return Ok();
		}

		private Task<ResourceResponse> ReplyAsync(Activity activity, string replyText)
		{
			string appId = configuration[MicrosoftAppCredentials.MicrosoftAppIdKey];
			string appPassword = configuration[MicrosoftAppCredentials.MicrosoftAppPasswordKey];
			MicrosoftAppCredentials appCredentials = new MicrosoftAppCredentials(appId, appPassword);
			ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl), appCredentials);
			Activity reply = activity.CreateReply(replyText);

			return connector.Conversations.ReplyToActivityAsync(reply);
		}

		private async Task AddScheduledMessageAsync(Activity activity, ISchedule schedule)
		{
			ScheduledMessage scheduledMessage = new ScheduledMessage
			{
				ConversationId = activity.Conversation.Id,
				Text = "Hello!",
				Schedule = schedule.Text
			};

			await context.ScheduledMessages.AddAsync(scheduledMessage);
			await context.SaveChangesAsync();
		}
	}
}