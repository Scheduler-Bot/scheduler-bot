using System.Collections.Generic;

namespace SchedulerBot.Database.Entities
{
	public class ScheduledMessage
	{
		public int Id { get; set; }

		public string Cron { get; set; }

		public string ConversationId { get; set; }

		public string Text { get; set; }

		public ICollection<ScheduledMessageRun> Runs { get; set; }
	}
}
