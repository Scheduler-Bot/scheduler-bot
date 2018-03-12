using System.Threading;
using System.Threading.Tasks;
using SchedulerBot.Database.Entities;

namespace SchedulerBot.Infrastructure.Interfaces.BotConnector
{
	public interface IMessageProcessor
	{
		Task SendMessageAsync(ScheduledMessage scheduledMessage, CancellationToken cancellationToken);
	}
}
