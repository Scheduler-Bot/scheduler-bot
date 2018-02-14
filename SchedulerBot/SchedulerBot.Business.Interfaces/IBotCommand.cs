using System.Threading.Tasks;

namespace SchedulerBot.Business.Interfaces
{
	public interface IBotCommand
	{
		string Name { get; }

		Task<CommandResult> ExecuteAsync(CommandExecutionContext context, string arguments);
	}
}
