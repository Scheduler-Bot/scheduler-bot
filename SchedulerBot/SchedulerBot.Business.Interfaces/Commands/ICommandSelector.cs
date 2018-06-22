namespace SchedulerBot.Business.Interfaces.Commands
{
	/// <summary>
	/// Defines an interface for selecting a command by its name.
	/// </summary>
	public interface ICommandSelector
	{
		/// <summary>
		/// Gets the command by the name.
		/// </summary>
		/// <param name="name">The command name.</param>
		/// <returns>The command selected by the provided name.</returns>
		IBotCommand GetCommand(string name);
	}
}
