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
		/// <returns></returns>
		IQueryable<T> GetAll();

		/// <summary>
		/// Gets the entity by an identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		T GetById(long id);

		/// <summary>
		/// Creates the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
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
		/// <returns></returns>
		bool Delete(long id);

		/// <summary>
		/// Deletes the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		void Delete(T entity);
	}
}
