using SchedulerBot.Database.Core;
using SchedulerBot.Database.Entities;
using SchedulerBot.Database.Interfaces.Repositories;

namespace SchedulerBot.Database.Repositories
{
	/// <inheritdoc cref="IScheduledMessageDetailsServiceUrlRepository"/>
	public class ScheduledMessageDetailsServiceUrlRepository : BaseRepository<ScheduledMessageDetailsServiceUrl>, IScheduledMessageDetailsServiceUrlRepository
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ScheduledMessageDetailsServiceUrlRepository"/> class.
		/// </summary>
		/// <param name="dbContext">The database context.</param>
		public ScheduledMessageDetailsServiceUrlRepository(SchedulerBotContext dbContext)
			: base(dbContext)
		{
		}
	}
}
