using System;
using System.Collections.Generic;
using SchedulerBot.Database.Entities.Enums;

namespace SchedulerBot.Database.Entities
{
	/// <summary>
	/// Represents the message scheduled by a user.
	/// </summary>
	public class ScheduledMessage
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets the text representation of the schedule.
		/// </summary>
		public string Schedule { get; set; }

		/// <summary>
		/// Gets or sets the text to be sent to the corresponding conversation.
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Gets or sets the state of the message.
		/// </summary>
		public ScheduledMessageState State { get; set; }

		/// <summary>
		/// Gets or sets the message details.
		/// </summary>
		public ScheduledMessageDetails Details { get; set; }

		/// <summary>
		/// Gets or sets the message events.
		/// </summary>
		public ICollection<ScheduledMessageEvent> Events { get; set; }
	}
}
