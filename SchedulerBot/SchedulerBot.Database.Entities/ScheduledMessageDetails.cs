using System;

namespace SchedulerBot.Database.Entities
{
	public class ScheduledMessageDetails
	{
		public Guid ScheduledMessageId { get; set; }

		public ScheduledMessage ScheduledMessage { get; set; }

		public string ServiceUrl { get; set; }

		public string FromId { get; set; }

		public string FromName { get; set; }

		public string RecipientId { get; set; }

		public string RecipientName { get; set; }

		public string ChannelId { get; set; }

		public string ConversationId { get; set; }

		public string Locale { get; set; }
	}
}
