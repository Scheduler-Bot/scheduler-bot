namespace SchedulerBot.Business.Interfaces
{
	public class CommandResult
	{
		public CommandResult(string text, bool succeeded)
		{
			Succeeded = succeeded;
			Text = text;
		}

		public string Text { get; }

		public bool Succeeded { get; }
	}
}
