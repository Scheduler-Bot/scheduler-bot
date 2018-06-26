using System.Linq;
using System.Text.RegularExpressions;

namespace SchedulerBot.Business.Entities
{
	/// <summary>
	/// Describes the base class for the result of the operation execution.
	/// </summary>
	public abstract class BaseExecutionResult
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BaseExecutionResult"/> class.
		/// </summary>
		/// <param name="errorCode">The error code.</param>
		/// <param name="errorMessage">The error message.</param>
		protected BaseExecutionResult(ExecutionErrorCode errorCode, string errorMessage)
		{
			ErrorCode = errorCode;
			ErrorMessage = string.IsNullOrEmpty(errorMessage)
				? errorMessage
				: GetDescription(errorCode);
		}

		/// <summary>
		/// Gets the ErrorCode value which is representing execution result.
		/// </summary>
		public ExecutionErrorCode ErrorCode { get; }

		/// <summary>
		/// Gets the error message related with the execution result.
		/// </summary>
		public string ErrorMessage { get; }

		/// <summary>
		/// Gets a value indicating whether the execution is successful.
		/// </summary>
		public bool IsSuccess => ErrorCode == ExecutionErrorCode.None;

		#region Private Methods

		/// <summary>
		/// Gets the description for <param name="executionErrorCode"/>.
		/// </summary>
		/// <param name="executionErrorCode">The execution error code.</param>
		/// <returns>
		/// The result will be a string split by Upper case letters.
		/// E.g. for InputCommandInvalidArguments result will be 'Input Command Invalid Arguments'
		/// </returns>
		private static string GetDescription(ExecutionErrorCode executionErrorCode)
		{
			string executionErrorCodeName = executionErrorCode.ToString("G");

			// Splits executionErrorCodeName the by upper case letters.
			// Solution was found by link https://stackoverflow.com/a/37532157/710014.
			string[] words = Regex.Matches(executionErrorCodeName, "(^[a-z]+|[A-Z]+(?![a-z])|[A-Z][a-z]+)")
				.Where(match => match != null)
				.Select(match => match.Value)
				.ToArray();
			string description = string.Join(" ", words);

			return description;
		}

		#endregion Private Methods
	}
}