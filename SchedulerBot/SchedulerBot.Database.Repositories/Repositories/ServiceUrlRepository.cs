using Microsoft.EntityFrameworkCore;
using SchedulerBot.Database.Entities;
using SchedulerBot.Database.Repositories;

namespace SchedulerBot.Database.Interfaces.Repositories
{
	/// <summary>
	/// Repository for <see cref = "ServiceUrl" />.
	/// </summary >
	public class ServiceUrlRepository : BaseRepository<ServiceUrl>, IServiceUrlRepository
	{
		public ServiceUrlRepository(DbContext dbContext)
			: base(dbContext)
		{
		}
	}
}
