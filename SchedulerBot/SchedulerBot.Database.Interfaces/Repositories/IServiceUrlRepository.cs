using System.Threading.Tasks;
using SchedulerBot.Database.Entities;

namespace SchedulerBot.Database.Interfaces.Repositories
{
	/// <summary>
	/// Repository for <see cref="ServiceUrl"/>.
	/// </summary>
	public interface IServiceUrlRepository : IRepository<ServiceUrl>
	{
		/// <summary>
		/// Gets <see cref="ServiceUrl"/> by <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns></returns>
		Task<ServiceUrl> GetByAddressAsync(string address);
	}
}
