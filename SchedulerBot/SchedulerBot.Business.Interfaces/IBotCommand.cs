using System.Threading.Tasks;
using Microsoft.Bot.Schema;

namespace SchedulerBot.Business.Interfaces
{
	public interface IBotCommand
	{
		string Name { get; }

		Task<string> ExecuteAsync(Activity activity, string arguments);
	}
}
