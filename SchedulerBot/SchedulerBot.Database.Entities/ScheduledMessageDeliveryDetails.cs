using System;

namespace SchedulerBot.Database.Entities
{
	/// <summary>
	/// Holds the information about channel, conversation and service URL.
	/// </summary>
	public class ScheduledMessageDeliveryDetails
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets the channel identifier.
		/// </summary>
		public string ChannelId { get; set; }

		/// <summary>
		/// Gets or sets the conversation identifier.
		/// </summary>
		public string ConversationId { get; set; }

		/// <summary>
		/// Gets or sets the service URL address.
		/// </summary>
		public string ServiceUrl { get; set; }

		/// <summary>
		/// Gets or sets the entity creation time.
		/// </summary>
		public DateTime CreatedOn { get; set; }
	}
}
