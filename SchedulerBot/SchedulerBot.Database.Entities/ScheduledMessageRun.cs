using System;

namespace SchedulerBot.Database.Entities
{
	public class ScheduledMessageRun
	{
		public int ScheduledMessageId { get; set; }

		public DateTime RanAt { get; set; }

		public ScheduledMessage ScheduledMessage { get; set; }
	}
}
