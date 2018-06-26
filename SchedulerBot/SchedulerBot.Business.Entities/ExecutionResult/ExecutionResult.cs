using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SchedulerBot.Business.Entities
{
	/// <summary>
	/// Describes the result of the operation execution.
	/// </summary>
	public class ExecutionResult
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ExecutionResult{T}"/> class.
		/// </summary>
		/// <param name="errorCode">The error code.</param>
		/// <param name="errorErrorMessage">The error message.</param>
		protected ExecutionResult(ExecutionErrorCode errorCode, string errorErrorMessage)
		{
			ErrorCode = errorCode;
			ErrorMessage = string.IsNullOrEmpty(errorErrorMessage)
				? errorErrorMessage
				: GetDescription(errorCode);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ExecutionResult"/> class.
		/// </summary>
		protected ExecutionResult(): this(ExecutionErrorCode.None, null)
		{
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

		/// <summary>
		/// Performs an implicit conversion from <see cref="ExecutionErrorCode"/> to <see cref="ExecutionResult"/>.
		/// </summary>
		/// <param name="errorCode">The error code.</param>
		/// <returns>
		/// The result of the conversion.
		/// </returns>
		public static implicit operator ExecutionResult(ExecutionErrorCode errorCode)
		{
			return Error(errorCode);
		}

		/// <summary>
		/// Creates the unsuccessful execution result.
		/// </summary>
		/// <param name="errorCode">The error code.</param>
		/// <param name="errorMessage">The error message.</param>
		/// <returns>
		/// A <see cref="ExecutionResult" /> instance with <see cref="ExecutionResult.IsSuccess" /> set to <c>false</c>.
		/// </returns>
		public static ExecutionResult Error(ExecutionErrorCode errorCode, string errorMessage = null)
		{
			return new ExecutionResult(errorCode, errorMessage);
		}

		/// <summary>
		/// Creates the successful execution result.
		/// </summary>
		/// <returns>
		/// A <see cref="ExecutionResult" /> instance with <see cref="ExecutionResult.IsSuccess" /> set to <c>true</c>.
		/// </returns>
		public static ExecutionResult Success()
		{
			return new ExecutionResult();
		}

		#region Private Methods

		/// <summary>
		/// Gets the description for <param name="executionErrorCode"/>.
		/// </summary>
		/// <param name="executionErrorCode">The execution error code.</param>
		/// <returns>
		/// If enum value marked by <see cref="DescriptionAttribute"/> then the resulted value will be obtained from it.
		/// In other case result will be a string split by Upper case letters.
		/// E.g. for InputCommandInvalidArguments result will be 'Input Command Invalid Arguments'
		/// </returns>
		private string GetDescription(ExecutionErrorCode executionErrorCode)
		{
			string executionErrorCodeName = executionErrorCode.ToString();
			FieldInfo fieldInfo = executionErrorCode.GetType().GetField(executionErrorCodeName);

			DescriptionAttribute[] descriptionAttributes =
				(DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

			string description = descriptionAttributes.Length > 0
				? descriptionAttributes[0].Description
				: SplitByUpperCase(executionErrorCodeName);

			return description;
		}

		/// <summary>
		/// Splits input <param name="value"/> the by upper case letters.
		/// Solution was found by link <see href="https://stackoverflow.com/a/37532157/710014"/>.
		/// </summary>
		/// <param name="value">The string which should be spitted.</param>
		private string SplitByUpperCase(string value)
		{
			string[] words = Regex.Matches(value, "(^[a-z]+|[A-Z]+(?![a-z])|[A-Z][a-z]+)")
				.Where(match => match != null)
				.Select(match => match.Value)
				.ToArray();
			string result = string.Join(" ", words);
			return result;
		}

		#endregion Private Methods
	}

	/// <summary>
	/// Describes the result of the operation execution.
	/// </summary>
	public class ExecutionResult<T>: ExecutionResult
		where T: class
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ExecutionResult{T}"/> class.
		/// </summary>
		/// <param name="entity">The entity.</param>
		private ExecutionResult(T entity)
		{
			Entity = entity;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ExecutionResult{T}"/> class.
		/// </summary>
		/// <param name="errorCode">The error code.</param>
		/// <param name="errorErrorMessage">The error message.</param>
		private ExecutionResult(ExecutionErrorCode errorCode, string errorErrorMessage)
			: base(errorCode, errorErrorMessage)
		{
			Entity = default(T);
		}

		/// <summary>
		/// Gets the entity which is related with execution result.
		/// </summary>
		public T Entity { get; }

		/// <summary>
		/// Performs an implicit conversion from <see cref="T" /> to a successful <see cref="ExecutionResult{T}" />.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>
		/// The result of the conversion.
		/// </returns>
		public static implicit operator ExecutionResult<T>(T entity)
		{
			return Success(entity);
		}

		/// <summary>
		/// Performs an implicit conversion from <see cref="ExecutionErrorCode"/> to <see cref="ExecutionResult{T}"/>.
		/// </summary>
		/// <param name="errorCode">The error code.</param>
		/// <returns>
		/// The result of the conversion.
		/// </returns>
		public static implicit operator ExecutionResult<T>(ExecutionErrorCode errorCode)
		{
			return Error(errorCode);
		}

		/// <summary>
		/// Creates the unsuccessful execution result.
		/// </summary>
		/// <param name="errorCode">The error code.</param>
		/// <param name="errorMessage">The error message.</param>
		/// <returns>
		/// A <see cref="ExecutionResult{T}" /> instance with <see cref="ExecutionResult.IsSuccess" /> set to <c>false</c>.
		/// </returns>
		public new static ExecutionResult<T> Error(ExecutionErrorCode errorCode, string errorMessage = null)
		{
			return new ExecutionResult<T>(errorCode, errorMessage);
		}

		/// <summary>
		/// Creates the successful execution result.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>
		/// A <see cref="ExecutionResult{T}" /> instance with <see cref="ExecutionResult.IsSuccess" /> set to <c>true</c>.
		/// </returns>
		public static ExecutionResult<T> Success(T entity)
		{
			return new ExecutionResult<T>(entity);
		}
	}
}
