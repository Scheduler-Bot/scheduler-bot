using Microsoft.Bot.Schema;
using SchedulerBot.Business.Interfaces.Entities;

namespace SchedulerBot.Business.Interfaces
{
	public interface ICommandRequestParser
	{
		CommandRequestParseResult Parse(Activity activity);
	}
}
