using Microsoft.EntityFrameworkCore;
using SchedulerBot.Database.Entities;
using SchedulerBot.Database.Interfaces.Repositories;

namespace SchedulerBot.Database.Repositories
{
	/// <inheritdoc cref="IScheduledMessageRepository"/>
	public class ScheduledMessageRepository : BaseRepository<ScheduledMessage>, IScheduledMessageRepository
	{
		public ScheduledMessageRepository(DbContext dbContext)
			: base(dbContext)
		{
		}
	}
}
