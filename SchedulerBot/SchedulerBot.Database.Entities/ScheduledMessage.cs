using System;
using SchedulerBot.Database.Entities.Enums;

namespace SchedulerBot.Database.Entities
{
	public class ScheduledMessage
	{
		public int Id { get; set; }

		public DateTime PerformAt { get; set; }

		public string ConversationId { get; set; }

		public string Message { get; set; }

		public ScheduledMessageState State { get; set; }
	}
}
