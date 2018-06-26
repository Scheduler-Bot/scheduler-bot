namespace SchedulerBot.Business.Entities
{
	/// <summary>
	/// Describes the result of the operation execution.
	/// </summary>
	public sealed class ExecutionResult : BaseExecutionResult
	{
		private ExecutionResult(ExecutionErrorCode errorCode, string errorMessage) : base(errorCode, errorMessage)
		{
		}

		private ExecutionResult(): this(ExecutionErrorCode.None, null)
		{
		}

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
	}

	/// <summary>
	/// Describes the result of the operation execution.
	/// </summary>
	public sealed class ExecutionResult<T>: BaseExecutionResult
		where T: class
	{
		private ExecutionResult(T entity) : this(ExecutionErrorCode.None, null)
		{
			Entity = entity;
		}

		private ExecutionResult(ExecutionErrorCode errorCode, string errorMessage)
			: base(errorCode, errorMessage)
		{
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
		public static ExecutionResult<T> Error(ExecutionErrorCode errorCode, string errorMessage = null)
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
