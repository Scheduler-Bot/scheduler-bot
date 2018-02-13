using System;

namespace SchedulerBot.Database.Entities
{
	public class ScheduledMessageLog
	{
		public DateTime CreatedOn { get; set; }

		public int ScheduledMessageId { get; set; }

		public ScheduledMessage ScheduledMessage { get; set; }
	}
}
