using System.Linq;
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
		/// Gets all entities.
		/// </summary>
		/// <returns>All entities.</returns>
		IQueryable<T> GetAll();

		/// <summary>
		/// Gets the entity by an identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>The found entity.</returns>
		T GetById(long id);

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
		void Update(T entity);

		/// <summary>
		/// Deletes entity with the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>The value indicating whether the entity has been deleted.</returns>
		bool Delete(long id);

		/// <summary>
		/// Deletes the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		void Delete(T entity);
	}
}
