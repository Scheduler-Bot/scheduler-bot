using System.Threading.Tasks;
using SchedulerBot.Database.Entities;

namespace SchedulerBot.Database.Interfaces.Repositories
{
	/// <summary>
	/// Repository for <see cref="ScheduledMessageEvent"/>.
	/// </summary>
	public interface IScheduledMessageEventRepository : IRepository<ScheduledMessageEvent>
	{
		/// <summary>
		/// Gets the next message event by <paramref name="conversationId"/>.
		/// </summary>
		/// <param name="conversationId">The conversation identifier.</param>
		/// <returns></returns>
		Task<ScheduledMessageEvent> GetNextMessageEventAsync(string conversationId);
	}
}
