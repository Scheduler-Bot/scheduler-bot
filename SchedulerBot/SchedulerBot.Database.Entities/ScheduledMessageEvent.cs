using System;

namespace SchedulerBot.Database.Entities
{
	public class ScheduledMessageEvent
	{
		public Guid Id { get; set; }

		public DateTime CreatedOn { get; set; }

		public Guid ScheduledMessageId { get; set; }

		public ScheduledMessage ScheduledMessage { get; set; }
	}
}
