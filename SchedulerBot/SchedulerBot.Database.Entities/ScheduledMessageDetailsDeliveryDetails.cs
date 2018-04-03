using System;

namespace SchedulerBot.Database.Entities
{
	/// <summary>
	/// Enables many-to-many relationship between <see cref="ScheduledMessageDetails"/> and <see cref="ScheduledMessageDeliveryDetails"/>.
	/// </summary>
	public class ScheduledMessageDetailsDeliveryDetails
	{
		/// <summary>
		/// Gets or sets the scheduled message details identifier.
		/// </summary>
		public Guid DetailsId { get; set; }

		/// <summary>
		/// Gets or sets the scheduled message details.
		/// </summary>
		public ScheduledMessageDetails MessageDetails { get; set; }

		/// <summary>
		/// Gets or sets the scheduled message delivery details identifier.
		/// </summary>
		public Guid DeliveryDetailsId { get; set; }

		/// <summary>
		/// Gets or sets the scheduled message delivery details.
		/// </summary>
		public ScheduledMessageDeliveryDetails DeliveryDetails { get; set; }
	}
}
