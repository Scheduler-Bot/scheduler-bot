using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using SchedulerBot.Business.Entities;
using SchedulerBot.Business.Interfaces;

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
		/// <param name="logger">The logger.</param>
		protected BotCommand(string name, ILogger logger)
		{
			Name = name;
			Logger = logger;
		}

		#endregion

		#region IBotCommand Implementation

		/// <inheritdoc />
		public string Name { get; }

		/// <inheritdoc />
		public async Task<CommandExecutionResult> ExecuteAsync(Activity activity, string arguments)
		{
			Logger.LogInformation("Executing '{0}' command with arguments '{1}'", Name, arguments);

			CommandExecutionResult executionResult = await ExecuteCoreAsync(activity, arguments);

			Logger.LogInformation("Finished executing '{0}' command", Name);

			return executionResult;
		}

		#endregion

		#region Protected Properties

		/// <summary>
		/// Gets the logger.
		/// </summary>
		protected ILogger Logger { get; }

		#endregion

		#region Abstract Methods

		/// <summary>
		/// When overridden in a derived class, executes the core command logic.
		/// </summary>
		/// <param name="activity">The activity.</param>
		/// <param name="arguments">The arguments.</param>
		/// <returns></returns>
		protected abstract Task<CommandExecutionResult> ExecuteCoreAsync(Activity activity, string arguments);

		#endregion
	}
}
