namespace SchedulerBot.Business.Entities
{
	public class CommandExecutionResult
	{
		private CommandExecutionResult(bool isSuccess, string text)
		{
			IsSuccess = isSuccess;
			Message = text;
		}

		public bool IsSuccess { get; }

		public string Message { get; }

		public static implicit operator CommandExecutionResult(string message)
		{
			return Success(message);
		}

		public static CommandExecutionResult Error(string errorMessage)
		{
			return new CommandExecutionResult(false, errorMessage);
		}

		public static CommandExecutionResult Success(string message)
		{
			return new CommandExecutionResult(false, message);
		}
	}
}
