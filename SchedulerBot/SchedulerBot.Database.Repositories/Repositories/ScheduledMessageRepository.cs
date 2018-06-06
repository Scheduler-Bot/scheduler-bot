using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchedulerBot.Database.Entities;
using SchedulerBot.Database.Entities.Enums;
using SchedulerBot.Database.Interfaces.Repositories;

namespace SchedulerBot.Database.Repositories
{
	/// <inheritdoc cref="IScheduledMessageRepository"/>
	public class ScheduledMessageRepository : BaseRepository<ScheduledMessage>, IScheduledMessageRepository
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ScheduledMessageRepository"/> class.
		/// </summary>
		/// <param name="dbContext">The database context.</param>
		public ScheduledMessageRepository(DbContext dbContext)
			: base(dbContext)
		{
		}

		/// <inheritdoc/>
		public async Task<IList<ScheduledMessage>> GetByConversationIdAndStateAsync(
			string conversationId,
			ScheduledMessageState state)
		{
			List<ScheduledMessage> result = await DbSet
				.Where(scheduledMessage =>
					scheduledMessage.State == state && scheduledMessage.Details.ConversationId == conversationId)
				.ToListAsync();

			return result;
		}

		/// <inheritdoc/>
		public async Task<ScheduledMessage> GetActiveByIdAndConversationIdAsync(
			string conversationId,
			Guid messageId)
		{
			ScheduledMessage result = await DbSet
				.Include(message => message.Details)
				.Include(message => message.Events)
				.FirstOrDefaultAsync(message =>
					message.Id == messageId &&
					message.State == ScheduledMessageState.Active &&
					message.Details.ConversationId == conversationId);

			return result;
		}

		/// <inheritdoc/>
		public async Task<ScheduledMessage> GetActiveByIdWithEventsAsync(Guid messageId)
		{
			ScheduledMessage result = await DbSet
				.Include(message => message.Events)
				.FirstOrDefaultAsync(message => message.Id == messageId && message.State == ScheduledMessageState.Active);
			return result;
		}
	}
}
