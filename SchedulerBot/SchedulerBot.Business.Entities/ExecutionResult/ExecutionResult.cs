namespace SchedulerBot.Business.Entities
{
	/// <summary>
	/// Describes the result of the operation execution.
	/// </summary>
	public class ExecutionResult
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ExecutionResult"/> class.
		/// </summary>
		protected ExecutionResult()
		{
			ErrorMessage = null;
			ErrorCode = ExecutionErrorCode.None;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ExecutionResult{T}"/> class.
		/// </summary>
		/// <param name="errorCode">The error code.</param>
		/// <param name="errorErrorMessage">The error message.</param>
		private ExecutionResult(ExecutionErrorCode errorCode, string errorErrorMessage)
		{
			ErrorMessage = errorErrorMessage;
			ErrorCode = errorCode;
		}

		/// <summary>
		/// Gets a value indicating whether the execution is successful.
		/// </summary>
		public bool IsSuccess => ErrorCode == ExecutionErrorCode.None;

		/// <summary>
		/// Gets the error message related with the execution result.
		/// </summary>
		public string ErrorMessage { get; protected set; }

		/// <summary>
		/// Gets the ErrorCode value which is representing execution result.
		/// </summary>
		public ExecutionErrorCode ErrorCode { get; protected set; }

		/// <summary>
		/// Creates the unsuccessful execution result.
		/// </summary>
		/// <param name="errorCode">The error code.</param>
		/// <param name="errorMessage">The error message.</param>
		/// <returns>
		/// A <see cref="ExecutionResult" /> instance with <see cref="ExecutionResult.IsSuccess" /> set to <c>false</c>.
		/// </returns>
		public static ExecutionResult Error(ExecutionErrorCode errorCode, string errorMessage)
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
			ErrorMessage = null;
			Entity = entity;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ExecutionResult{T}"/> class.
		/// </summary>
		/// <param name="errorCode">The error code.</param>
		/// <param name="errorErrorMessage">The error message.</param>
		private ExecutionResult(ExecutionErrorCode errorCode, string errorErrorMessage)
		{
			ErrorMessage = errorErrorMessage;
			ErrorCode = errorCode;
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
		/// Creates the unsuccessful execution result.
		/// </summary>
		/// <param name="errorCode">The error code.</param>
		/// <param name="errorMessage">The error message.</param>
		/// <returns>
		/// A <see cref="ExecutionResult{T}" /> instance with <see cref="ExecutionResult.IsSuccess" /> set to <c>false</c>.
		/// </returns>
		public static new ExecutionResult<T> Error(ExecutionErrorCode errorCode, string errorMessage)
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
