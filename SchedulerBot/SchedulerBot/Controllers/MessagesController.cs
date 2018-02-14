using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;
using Microsoft.Rest;
using SchedulerBot.Database.Core;
using SchedulerBot.Database.Entities;
using SchedulerBot.Infrastructure.Interfaces;

namespace SchedulerBot.Controllers
{
	[Route("api/[controller]")]
	public class MessagesController : Controller
	{
		private readonly SchedulerBotContext context;
		private readonly ServiceClientCredentials credentials;
		private readonly IScheduleParser scheduleParser;

		public MessagesController(
			SchedulerBotContext context,
			ServiceClientCredentials credentials,
			IScheduleParser scheduleParser)
		{
			this.context = context;
			this.credentials = credentials;
			this.scheduleParser = scheduleParser;
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
			Activity reply = activity.CreateReply(replyText);

			using (ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl), credentials))
			{
				return connector.Conversations.ReplyToActivityAsync(reply);
			}
		}

		private async Task AddScheduledMessageAsync(Activity activity, ISchedule schedule)
		{
			ScheduledMessage scheduledMessage = new ScheduledMessage
			{
				Text = "Hello!",
				Schedule = schedule.Text,
				Details = CreateMessageDetails(activity)
			};

			await context.ScheduledMessages.AddAsync(scheduledMessage);
			await context.SaveChangesAsync();
		}

		private static ScheduledMessageDetails CreateMessageDetails(Activity activity)
		{
			return new ScheduledMessageDetails
			{
				ServiceUrl = activity.ServiceUrl,
				FromId = activity.Recipient.Id,
				FromName = activity.Recipient.Name,
				RecipientId = activity.From.Id,
				RecipientName = activity.From.Name,
				ChannelId = activity.ChannelId,
				ConversationId = activity.Conversation.Id,
				Locale = activity.Locale
			};
		}
	}
}