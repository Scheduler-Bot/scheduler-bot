namespace SchedulerBot.Business.Entities
{
	/// <summary>
	/// Execution Error Codes. This enum used inside <see cref="ExecutionResult" />.
	/// </summary>
	public enum ExecutionErrorCode
	{
		/// <summary>
		/// None value specified. Default value for <see cref="ExecutionErrorCode"/> enum.
		/// </summary>
		None = 0,

		#region 100#### Authentication

		#endregion 100#### Authentication

		#region 101#### Validation

		#region 10101## Validation - Input Command

		/// <summary>
		/// The input command has invalid arguments.
		/// </summary>
		InputCommandInvalidArguments = 1010101,

		#endregion 10101## Validation - Input Command

		#region 10102## Validation - Scheduler CRON

		/// <summary>
		/// Scheduler Cron expression cannot be parsed due invalid format.
		/// </summary>
		SchedulerCronCannotRecognize = 1010201,

		#endregion 101## Validation - Scheduler CRON

		#endregion 101#### Validation

		#region 102#### Business Logic

		#region 10201## Business Logic - Scheduler

		/// <summary>
		/// The SchedulerMessage cannot be found.
		/// </summary>
		SchedulerMessageCannotBeFound = 1020101,

		/// <summary>
		/// The ManageConversationLink cannot be found.
		/// </summary>
		ManageConversationLinkCannotBeFound = 1020102,

		#endregion 10201## Business Logic - Scheduler

		#endregion 102#### Business Logic
	}
}
