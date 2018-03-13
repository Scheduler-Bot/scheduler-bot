namespace SchedulerBot.Business.Entities
{
	/// <summary>
	/// Describes the result of the command execution.
	/// </summary>
	public class CommandExecutionResult
	{
		private CommandExecutionResult(bool isSuccess, string text)
		{
			IsSuccess = isSuccess;
			Message = text;
		}

		/// <summary>
		/// Gets a value indicating whether the command execution is successful.
		/// </summary>
		public bool IsSuccess { get; }

		/// <summary>
		/// Gets the message representing the command execution result.
		/// </summary>
		public string Message { get; }

		/// <summary>
		/// Performs an implicit conversion from <see cref="string"/> to a successful <see cref="CommandExecutionResult"/>.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator CommandExecutionResult(string message)
		{
			return Success(message);
		}

		/// <summary>
		/// Creates the unsuccessful command execution result.
		/// </summary>
		/// <param name="errorMessage">The error message.</param>
		/// <returns>A <see cref="CommandExecutionResult"/> instance with <see cref="IsSuccess"/> set to <c>false</c>.</returns>
		public static CommandExecutionResult Error(string errorMessage)
		{
			return new CommandExecutionResult(false, errorMessage);
		}

		/// <summary>
		/// Creates the successful command execution result.
		/// </summary>
		/// <param name="message">The resulting message.</param>
		/// <returns>A <see cref="CommandExecutionResult"/> instance with <see cref="IsSuccess"/> set to <c>true</c>.</returns>
		public static CommandExecutionResult Success(string message)
		{
			return new CommandExecutionResult(false, message);
		}
	}
}
