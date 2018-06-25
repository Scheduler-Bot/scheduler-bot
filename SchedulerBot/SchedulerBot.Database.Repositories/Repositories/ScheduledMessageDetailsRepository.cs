using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SchedulerBot.Database.Core;
using SchedulerBot.Database.Entities;
using SchedulerBot.Database.Interfaces.Repositories;

namespace SchedulerBot.Database.Repositories
{
	/// <inheritdoc cref="IScheduledMessageDetailsRepository"/>
	public class ScheduledMessageDetailsRepository : BaseRepository<ScheduledMessageDetails>, IScheduledMessageDetailsRepository
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ScheduledMessageDetailsRepository"/> class.
		/// </summary>
		/// <param name="dbContext">The database context.</param>
		public ScheduledMessageDetailsRepository(SchedulerBotContext dbContext)
			: base(dbContext)
		{
		}

		/// <inheritdoc />
		public async Task<IList<ScheduledMessageDetails>> GetScheduledMessageDetailsAsync(
			string channelId,
			string conversationId,
			bool includeServiceUrls)
		{
			var scheduledMessageDetails = DbSet.Include(details => details.DetailsServiceUrls);

			IQueryable<ScheduledMessageDetails> result =
				includeServiceUrls
					? FilterByConversation(scheduledMessageDetails.ThenInclude(detailsServiceUrl => detailsServiceUrl.ServiceUrl), channelId, conversationId)
					: FilterByConversation(scheduledMessageDetails, channelId, conversationId);

			return await result.ToListAsync();
		}

		/// <inheritdoc />
		public async Task<IList<ScheduledMessageDetails>> GetScheduledMessageDetailsWithEventsAsync(string channelId, string conversationId)
		{
			var scheduledMessageDetails = DbSet
				.Include(messageDetails => messageDetails.ScheduledMessage)
				.ThenInclude(message => message.Events);

			IQueryable<ScheduledMessageDetails> result = FilterByConversation(scheduledMessageDetails, channelId, conversationId);

			return await result.ToListAsync();
		}
		

		private static IQueryable<ScheduledMessageDetails> FilterByConversation(
			IQueryable<ScheduledMessageDetails> scheduledMessageDetails,
			string channelId,
			string conversationId)
		{
			return scheduledMessageDetails
				.Where(details =>
					details.ChannelId.ToUpper(CultureInfo.InvariantCulture) == channelId.ToUpper(CultureInfo.InvariantCulture) &&
					details.ConversationId.ToUpper(CultureInfo.InvariantCulture) == conversationId.ToUpper(CultureInfo.InvariantCulture));
		}
	}
}
