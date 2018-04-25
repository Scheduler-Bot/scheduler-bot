using System;
using SchedulerBot.Database.Entities.Enums;
using SchedulerBot.Database.Entities.Interfaces;

namespace SchedulerBot.Database.Entities
{
	/// <summary>
	/// Represents a single occurrence of the scheduled message.
	/// </summary>
	public class ScheduledMessageEvent : ICreatedOn
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets the event creation time.
		/// </summary>
		public DateTime CreatedOn { get; set; }

		/// <summary>
		/// Gets or sets the next event occurrence.
		/// </summary>
		public DateTime NextOccurrence { get; set; }

		/// <summary>
		/// Gets or sets the event state.
		/// </summary>
		public ScheduledMessageEventState State { get; set; }

		/// <summary>
		/// Gets or sets the associated scheduled message identifier.
		/// </summary>
		public Guid ScheduledMessageId { get; set; }

		/// <summary>
		/// Gets or sets the associated scheduled message.
		/// </summary>
		public ScheduledMessage ScheduledMessage { get; set; }
	}
}
