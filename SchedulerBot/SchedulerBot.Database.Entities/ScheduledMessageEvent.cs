using System;
using SchedulerBot.Database.Entities.Enums;

namespace SchedulerBot.Database.Entities
{
	public class ScheduledMessageEvent
	{
		public Guid Id { get; set; }

		public DateTime CreatedOn { get; set; }

		public DateTime NextOccurence { get; set; }

		public ScheduledMessageEventState State { get; set; }

		public Guid ScheduledMessageId { get; set; }

		public ScheduledMessage ScheduledMessage { get; set; }
	}
}
