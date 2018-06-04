using Microsoft.EntityFrameworkCore;
using SchedulerBot.Database.Entities;
using SchedulerBot.Database.Interfaces.Repositories;

namespace SchedulerBot.Database.Repositories
{
	/// <summary>
	/// Repository for <see cref = "ScheduledMessageDetailsServiceUrl" />.
	/// </summary >
	public class ScheduledMessageDetailsServiceUrlRepository : BaseRepository<ScheduledMessageDetailsServiceUrl>, IScheduledMessageDetailsServiceUrlRepository
	{
		public ScheduledMessageDetailsServiceUrlRepository(DbContext dbContext)
			: base(dbContext)
		{
		}
	}
}
