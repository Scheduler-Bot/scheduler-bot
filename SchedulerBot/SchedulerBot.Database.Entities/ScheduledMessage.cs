using System;
using System.Collections.Generic;

namespace SchedulerBot.Database.Entities
{
	public class ScheduledMessage
	{
		public Guid Id { get; set; }

		public string Schedule { get; set; }

		public string ConversationId { get; set; }

		public string Text { get; set; }

		public ICollection<ScheduledMessageLog> Logs { get; set; }
	}
}
