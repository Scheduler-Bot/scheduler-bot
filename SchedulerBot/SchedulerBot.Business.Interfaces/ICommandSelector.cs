namespace SchedulerBot.Business.Interfaces
{
	public interface ICommandSelector
	{
		IBotCommand GetCommand(string name);
	}
}
