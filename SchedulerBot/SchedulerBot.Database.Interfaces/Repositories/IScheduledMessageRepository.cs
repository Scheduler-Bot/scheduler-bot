using System.Collections.Generic;
using System.Threading.Tasks;
using SchedulerBot.Database.Entities;
using SchedulerBot.Database.Entities.Enums;

namespace SchedulerBot.Database.Interfaces.Repositories
{
	/// <summary>
	/// Repository for <see cref="ScheduledMessage"/>.
	/// </summary>
	public interface IScheduledMessageRepository : IRepository<ScheduledMessage>
	{
		/// <summary>
		/// Gets the list of ScheduledMessages by <paramref name="conversationId"/> and <paramref name="state"/>.
		/// </summary>
		/// <param name="conversationId">The conversation identifier.</param>
		/// <param name="state">The state.</param>
		/// <returns></returns>
		Task<IList<ScheduledMessage>> GetByConversationIdAndStateAsync(
			string conversationId,
			ScheduledMessageState state);
	}
}
