namespace SchedulerBot.Business.Entities
{
	/// <summary>
	/// Execution Error Codes. This enum used inside <see cref="ExecutionResult"/>.
	/// </summary>
	public enum ExecutionErrorCode
	{
		None = 0,

		#region 100#### Authentication

		#endregion 100#### Authentication

		#region 101#### Validation

		#region 10101## Validation - Input Command

		InputCommandInvalidArguments = 1010101,

		#endregion 10101## Validation - Input Command

		#region 10102## Validation - Scheduler CRON

		SchedulerCronCannotRecognize = 1010201,

		#endregion 101## Validation - Scheduler CRON

		#endregion 101#### Validation

		#region 102#### Business Logic

		#region 10201## Business Logic - Scheduler

		SchedulerMessageCannotBeFound = 1020101,
		ManageConversationLinkCannotBeFound = 1020102,

		#endregion 10201## Business Logic - Scheduler

		#endregion 102#### Business Logic
	}
}
