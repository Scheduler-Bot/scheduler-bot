using System.Collections.Generic;
using System.Threading.Tasks;
using SchedulerBot.Database.Entities;

namespace SchedulerBot.Database.Interfaces.Repositories
{
	/// <summary>
	/// Repository for <see cref="ScheduledMessageDetails"/>.
	/// </summary>
	public interface IScheduledMessageDetailsRepository : IRepository<ScheduledMessageDetails>
	{
		/// <summary>
		/// Gets the scheduled message details for the specified channel and conversation.
		/// </summary>
		/// <param name="channelId">The channel identifier.</param>
		/// <param name="conversationId">The conversation identifier.</param>
		/// <param name="includeServiceUrls">
		/// if set to <c>true</c> includes the <see cref="ScheduledMessageDetailsServiceUrl.ServiceUrl"/> on <see cref="ScheduledMessageDetails.DetailsServiceUrls"/>
		/// .</param>
		/// <returns>
		/// A task that represents the asynchronous operation.
		/// The task result contains the collection of found <see cref="ScheduledMessageDetails"/> instances.
		/// </returns>
		Task<IList<ScheduledMessageDetails>> GetScheduledMessageDetails(
			string channelId,
			string conversationId,
			bool includeServiceUrls);
	}
}
