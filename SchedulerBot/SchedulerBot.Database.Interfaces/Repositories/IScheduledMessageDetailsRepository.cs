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
		Task<IList<ScheduledMessageDetails>> GetScheduledMessageDetails(
			string channelId,
			string conversationId,
			bool includeServiceUrls);
	}
}
