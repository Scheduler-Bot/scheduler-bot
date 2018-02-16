using Microsoft.Bot.Schema;

namespace SchedulerBot.Business.Interfaces
{
	public interface ICommandRequestParser
	{
		CommandRequestParseResult Parse(Activity activity);
	}
}
