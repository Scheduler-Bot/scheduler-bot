namespace SchedulerBot.Database.Entities.Enums
{
	/// <summary>
	/// Describes the schedule message event state.
	/// </summary>
	public enum ScheduledMessageEventState
	{
		/// <summary>
		/// The event is in pending state and the corresponding message is to be sent to the corresponding conversation.
		/// </summary>
		Pending = 1,

		/// <summary>
		/// The event is completed and the associated message has been sent.
		/// </summary>
		Completed = 2,

		/// <summary>
		/// The event execution has failed.
		/// </summary>
		Failed = 3,

		/// <summary>
		/// The event has been deleted.
		/// </summary>
		Deleted = 4
	}
}
