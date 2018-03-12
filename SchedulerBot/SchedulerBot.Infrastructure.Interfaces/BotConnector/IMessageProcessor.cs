using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using SchedulerBot.Database.Entities;

namespace SchedulerBot.Infrastructure.Interfaces.BotConnector
{
	public interface IMessageProcessor
	{
		Task SendMessageAsync(ScheduledMessage scheduledMessage, CancellationToken cancellationToken);

		Task<ResourceResponse> ReplyAsync(Activity activity, string replyText);
	}
}
