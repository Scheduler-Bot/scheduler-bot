using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchedulerBot.Database.Core;
using SchedulerBot.Database.Entities;
using SchedulerBot.Database.Entities.Enums;
using SchedulerBot.Database.Interfaces.Repositories;

namespace SchedulerBot.Database.Repositories
{
	/// <inheritdoc cref="IScheduledMessageEventRepository"/>
	public class ScheduledMessageEventRepository : BaseRepository<ScheduledMessageEvent>, IScheduledMessageEventRepository
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ScheduledMessageEventRepository"/> class.
		/// </summary>
		/// <param name="dbContext">The database context.</param>
		public ScheduledMessageEventRepository(SchedulerBotContext dbContext)
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
					@event.ScheduledMessage.Details.ConversationId == conversationId)
				.OrderBy(@event => @event.NextOccurrence)
				.FirstOrDefaultAsync();

			return result;
		}

		/// <inheritdoc/>
		public async Task<IList<ScheduledMessageEvent>> GetAllPendingWithScheduledMessages(DateTime tillTime)
		{
			List<ScheduledMessageEvent> result = await DbSet
				.Where(@event => @event.State == ScheduledMessageEventState.Pending && @event.NextOccurrence < tillTime)
				.Include(@event => @event.ScheduledMessage)
				.ThenInclude(message => message.Details)
				.ThenInclude(details => details.DetailsServiceUrls)
				.ThenInclude(detailsServiceUrl => detailsServiceUrl.ServiceUrl)
				.ToListAsync();

			return result;
		}
	}
}
