using Microsoft.EntityFrameworkCore;
using SchedulerBot.Database.Entities;
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
	}
}
