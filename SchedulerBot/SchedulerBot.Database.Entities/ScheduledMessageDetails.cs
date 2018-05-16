using System;
using System.Collections.Generic;

namespace SchedulerBot.Database.Entities
{
	/// <summary>
	/// Represents scheduled message details.
	/// </summary>
	public class ScheduledMessageDetails
	{
		/// <summary>
		/// Gets or sets the associated scheduled message identifier.
		/// </summary>
		public Guid ScheduledMessageId { get; set; }

		/// <summary>
		/// Gets or sets the associated scheduled message.
		/// </summary>
		public ScheduledMessage ScheduledMessage { get; set; }

		/// <summary>
		/// Gets or sets the identifier of the message sender - the bot.
		/// </summary>
		public string FromId { get; set; }

		/// <summary>
		/// Gets or sets the name of the message sender - the bot.
		/// </summary>
		public string FromName { get; set; }

		/// <summary>
		/// Gets or sets the recipient identifier.
		/// </summary>
		public string RecipientId { get; set; }

		/// <summary>
		/// Gets or sets the name of the recipient.
		/// </summary>
		public string RecipientName { get; set; }

		/// <summary>
		/// Gets or sets the channel identifier.
		/// </summary>
		public string ChannelId { get; set; }

		/// <summary>
		/// Gets or sets the conversation identifier.
		/// </summary>
		public string ConversationId { get; set; }

		/// <summary>
		/// Gets or sets the locale of the recipient.
		/// </summary>
		public string Locale { get; set; }

		/// <summary>
		/// Gets or sets the UTC offset of the recipient time zone.
		/// </summary>
		public TimeSpan? TimeZoneOffset { get; set; }

		/// <summary>
		/// Gets or sets the collection of <see cref="ScheduledMessageDetailsServiceUrl"/> objects
		/// connecting the current instance to corresponding <see cref="ServiceUrl"/> objects.
		/// </summary>
		public ICollection<ScheduledMessageDetailsServiceUrl> DetailsServiceUrls { get; set; }
	}
}
