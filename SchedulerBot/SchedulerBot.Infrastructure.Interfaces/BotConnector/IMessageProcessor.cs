using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using SchedulerBot.Database.Entities;

namespace SchedulerBot.Infrastructure.Interfaces.BotConnector
{
	/// <summary>
	/// Defines an interface for processing messages.
	/// </summary>
	public interface IMessageProcessor
	{
		/// <summary>
		/// Sends the message asynchronously.
		/// </summary>
		/// <param name="scheduledMessage">The scheduled message.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task that represents the message sending.</returns>
		Task SendMessageAsync(ScheduledMessage scheduledMessage, CancellationToken cancellationToken);

		/// <summary>
		/// Replies to the user message asynchronously.
		/// </summary>
		/// <param name="activity">The activity.</param>
		/// <param name="replyText">The reply text.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task that represents the reply.</returns>
		Task<ResourceResponse> ReplyAsync(Activity activity, string replyText, CancellationToken cancellationToken);
	}
}
