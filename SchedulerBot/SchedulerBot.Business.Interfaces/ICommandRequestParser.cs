namespace SchedulerBot.Business.Interfaces
{
	public interface ICommandRequestParser
	{
		CommandRequestParseResult Parse(string inputText);
	}
}
