using Microsoft.EntityFrameworkCore;
using SchedulerBot.Database.Entities;
using SchedulerBot.Database.Interfaces.Repositories;

namespace SchedulerBot.Database.Repositories
{
	/// <summary>
	/// Repository for <see cref = "ScheduledMessageDetails" />.
	/// </summary >
	public class ScheduledMessageDetailsRepository : BaseRepository<ScheduledMessageDetails>, IScheduledMessageDetailsRepository
	{
		public ScheduledMessageDetailsRepository(DbContext dbContext)
			: base(dbContext)
		{
		}
	}
}
