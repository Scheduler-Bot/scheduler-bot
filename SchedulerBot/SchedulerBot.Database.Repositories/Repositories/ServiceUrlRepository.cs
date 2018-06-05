using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchedulerBot.Database.Entities;
using SchedulerBot.Database.Interfaces.Repositories;

namespace SchedulerBot.Database.Repositories
{
	/// <inheritdoc cref="IServiceUrlRepository"/>
	public class ServiceUrlRepository : BaseRepository<ServiceUrl>, IServiceUrlRepository
	{
		public ServiceUrlRepository(DbContext dbContext)
			: base(dbContext)
		{
		}

		/// <inheritdoc/>
		public async Task<ServiceUrl> GetByAddressAsync(string address)
		{
			ServiceUrl serviceUrl =
				await DbSet.FirstOrDefaultAsync(url => url.Address.Equals(address, StringComparison.OrdinalIgnoreCase));
			return serviceUrl;
		}
	}
}
