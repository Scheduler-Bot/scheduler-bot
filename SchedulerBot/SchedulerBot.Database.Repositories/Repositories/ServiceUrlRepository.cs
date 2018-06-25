using System.Globalization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchedulerBot.Database.Core;
using SchedulerBot.Database.Entities;
using SchedulerBot.Database.Interfaces.Repositories;

namespace SchedulerBot.Database.Repositories
{
	/// <inheritdoc cref="IServiceUrlRepository"/>
	public class ServiceUrlRepository : BaseRepository<ServiceUrl>, IServiceUrlRepository
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceUrlRepository"/> class.
		/// </summary>
		/// <param name="dbContext">The database context.</param>
		public ServiceUrlRepository(SchedulerBotContext dbContext)
			: base(dbContext)
		{
		}

		/// <inheritdoc/>
		public async Task<ServiceUrl> GetByAddressAsync(string address)
		{
			ServiceUrl serviceUrl = await DbSet.FirstOrDefaultAsync(url => url.Address.ToUpper(CultureInfo.InvariantCulture) == address.ToUpper(CultureInfo.InvariantCulture));
			return serviceUrl;
		}
	}
}
