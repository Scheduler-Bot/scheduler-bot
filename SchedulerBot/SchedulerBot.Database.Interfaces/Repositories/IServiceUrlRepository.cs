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
		/// <returns>
		/// A task that represents the asynchronous operation.
		/// The task result contains the found <see cref="ServiceUrl"/> instance.
		/// </returns>
		Task<ServiceUrl> GetByAddressAsync(string address);
	}
}
