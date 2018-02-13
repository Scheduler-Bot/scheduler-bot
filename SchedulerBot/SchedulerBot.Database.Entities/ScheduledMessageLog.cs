using System;

namespace SchedulerBot.Database.Entities
{
	public class ScheduledMessageLog
	{
		public Guid Id { get; set; }

		public DateTime CreatedOn { get; set; }

		public Guid ScheduledMessageId { get; set; }

		public ScheduledMessage ScheduledMessage { get; set; }
	}
}
