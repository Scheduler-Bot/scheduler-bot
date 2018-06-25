using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using SchedulerBot.Business.Entities;
using SchedulerBot.Business.Interfaces.Commands;
using SchedulerBot.Database.Interfaces;

namespace SchedulerBot.Business.Commands
{
	/// <summary>
	/// Basic implementation of <see cref="IBotCommand"/> interface.
	/// </summary>
	/// <seealso cref="IBotCommand" />
	public abstract class BotCommand : IBotCommand
	{
		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="BotCommand"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="unitOfWork">The unit of work.</param>
		/// <param name="logger">The logger.</param>
		protected BotCommand(string name, IUnitOfWork unitOfWork,  ILogger logger)
		{
			Name = name;
			UnitOfWork = unitOfWork;
			Logger = logger;
		}

		#endregion

		#region Public Properties

		/// <inheritdoc />
		public string Name { get; }

		#endregion

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

		#region Public Methods

		/// <inheritdoc />
		public async Task<ExecutionResult<string>> ExecuteAsync(Activity activity, string arguments)
		{
			Logger.LogInformation("Executing '{0}' command with arguments '{1}'", Name, arguments);

			ExecutionResult<string> executionResult = await ExecuteCoreAsync(activity, arguments);

			Logger.LogInformation("Finished executing '{0}' command", Name);

			return executionResult;
		}

		#endregion

		#region Abstract Methods

		/// <summary>
		/// When overridden in a derived class, executes the core command logic.
		/// </summary>
		/// <param name="activity">The activity.</param>
		/// <param name="arguments">The arguments.</param>
		/// <returns>The result of the command execution.</returns>
		protected abstract Task<ExecutionResult<string>> ExecuteCoreAsync(Activity activity, string arguments);

		#endregion
	}
}
