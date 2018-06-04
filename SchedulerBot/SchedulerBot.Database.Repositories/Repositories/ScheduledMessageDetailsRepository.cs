using Microsoft.EntityFrameworkCore;
using SchedulerBot.Database.Entities;
using SchedulerBot.Database.Interfaces.Repositories;

namespace SchedulerBot.Database.Repositories
{
	/// <inheritdoc cref="IScheduledMessageDetailsRepository"/>
	public class ScheduledMessageDetailsRepository : BaseRepository<ScheduledMessageDetails>, IScheduledMessageDetailsRepository
	{
		public ScheduledMessageDetailsRepository(DbContext dbContext)
			: base(dbContext)
		{
		}
	}
}
