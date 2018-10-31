using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;

namespace SchedulerBot.Bots
{
	public class SchedulerBot : IBot
	{
		/// <summary>
		/// Every conversation turn for our Scheduler Bot will call this method.
		/// There are no dialogs used, since it's "single turn" processing, meaning a single
		/// request and response.
		/// </summary>
		/// <param name="turnContext">A <see cref="ITurnContext"/> containing all the data needed
		/// for processing this conversation turn. </param>
		/// <param name="cancellationToken">(Optional) A <see cref="CancellationToken"/> that can be used by other objects
		/// or threads to receive notice of cancellation.</param>
		/// <returns>A <see cref="Task"/> that represents the work queued to execute.</returns>
		/// <seealso cref="BotStateSet"/>
		/// <seealso cref="ConversationState"/>
		/// <seealso cref="IMiddleware"/>
		public Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = new CancellationToken())
		{
			throw new NotImplementedException();
		}
	}
}
