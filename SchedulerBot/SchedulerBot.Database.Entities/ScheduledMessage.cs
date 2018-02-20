using System;
using System.Collections.Generic;
using SchedulerBot.Database.Entities.Enums;

namespace SchedulerBot.Database.Entities
{
	public class ScheduledMessage
	{
		public Guid Id { get; set; }

		public string Schedule { get; set; }

		public string Text { get; set; }

		public ScheduledMessageState State { get; set; }

		public ScheduledMessageDetails Details { get; set; }

		public ICollection<ScheduledMessageEvent> Events { get; set; }
	}
}
