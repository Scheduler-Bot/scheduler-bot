using System.Collections.Generic;
using Microsoft.Bot.Schema;
using SchedulerBot.Database.Entities;

namespace SchedulerBot.Business.Interfaces
{
	public interface IMessageListFormatter
	{
		string Format(IEnumerable<ScheduledMessage> scheduledMessages, Activity activity);
	}
}
