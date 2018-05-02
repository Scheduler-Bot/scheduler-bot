using System;
using System.Collections.Generic;
using SchedulerBot.Database.Entities.Enums;

namespace SchedulerBot.Models
{
	/// <summary>
	/// Represents information about a scheduled message returned to a user.
	/// </summary>
	public class ScheduledMessageModel
	{
		/// <summary>
		/// Gets or sets the scheduled message identifier.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets the scheduled message text.
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Gets or sets the scheduled message state.
		/// </summary>
		public ScheduledMessageState State { get; set; }

		/// <summary>
		/// Gets or sets the locale which the scheduled message uses.
		/// </summary>
		public string Locale { get; set; }

		/// <summary>
		/// Gets or sets the next occurrences of the scheduled message.
		/// </summary>
		public IList<DateTime> NextOccurrences { get; set; }
	}
}
