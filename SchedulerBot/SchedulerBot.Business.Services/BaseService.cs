using Microsoft.Extensions.Logging;
using SchedulerBot.Database.Interfaces;

namespace SchedulerBot.Business.Services
{
	/// <summary>
	/// Basic implementation of services.
	/// </summary>
	public abstract class BaseService
	{
		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseService"/> class.
		/// </summary>
		/// <param name="unitOfWork">The unit of work.</param>
		/// <param name="logger">The logger.</param>
		protected BaseService(IUnitOfWork unitOfWork, ILogger logger)
		{
			UnitOfWork = unitOfWork;
			Logger = logger;
		}

		#endregion Constructor

		#region Protected Properties

		/// <summary>
		/// Gets the Unit of Work to work with a Database.
		/// </summary>
		protected IUnitOfWork UnitOfWork { get; }

		/// <summary>
		/// Gets the logger.
		/// </summary>
		protected ILogger Logger { get; }

		#endregion
	}
}
