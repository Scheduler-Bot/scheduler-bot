using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchedulerBot.Database.Entities;
using SchedulerBot.Database.Entities.Enums;
using SchedulerBot.Database.Interfaces.Repositories;

namespace SchedulerBot.Database.Repositories
{
	/// <inheritdoc cref="IScheduledMessageEventRepository"/>
	public class ScheduledMessageEventRepository : BaseRepository<ScheduledMessageEvent>, IScheduledMessageEventRepository
	{
		public ScheduledMessageEventRepository(DbContext dbContext)
			: base(dbContext)
		{
		}

		/// <inheritdoc/>
		public async Task<ScheduledMessageEvent> GetNextMessageEventAsync(string conversationId)
		{
			ScheduledMessageEvent result = await DbSet
				.Include(@event => @event.ScheduledMessage)
					.ThenInclude(message => message.Details)
				.Where(@event =>
					@event.State == ScheduledMessageEventState.Pending &&
					@event.ScheduledMessage.State == ScheduledMessageState.Active &&
					@event.ScheduledMessage.Details.ConversationId.Equals(conversationId, StringComparison.Ordinal))
				.OrderBy(@event => @event.NextOccurrence)
				.FirstOrDefaultAsync();
			return result;
		}
	}
}
