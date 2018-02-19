using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using SchedulerBot.Business.Interfaces.Entities;

namespace SchedulerBot.Business.Interfaces
{
	public interface IBotCommand
	{
		string Name { get; }

		Task<CommandExecutionResult> ExecuteAsync(Activity activity, string arguments);
	}
}
