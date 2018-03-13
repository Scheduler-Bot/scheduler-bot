namespace SchedulerBot.Database.Entities.Enums
{
	/// <summary>
	/// Describes the scheduled message state.
	/// </summary>
	public enum ScheduledMessageState
	{
		/// <summary>
		/// The message is active and is being sent accordingly to its schedule.
		/// </summary>
		Active = 1,

		/// <summary>
		/// The message has been deleted and is not being sent.
		/// </summary>
		Deleted = 2
	}
}
