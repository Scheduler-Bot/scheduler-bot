using System.Threading.Tasks;

namespace SchedulerBot.Database.Interfaces
{
	/// <summary>
	/// Base Repositories functionality.
	/// </summary>
	/// <typeparam name="T">Entity type.</typeparam>
	public interface IRepository<T> where T : class
	{
		/// <summary>
		/// Creates the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>
		/// A task that represents the asynchronous Add operation. The task result contains the added entity.
		/// </returns>
		Task<T> AddAsync(T entity);

		/// <summary>
		/// Updates the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>The updated entity.</returns>
		T Update(T entity);

		/// <summary>
		/// Deletes the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>The deleted entity.</returns>
		T Delete(T entity);
	}
}
