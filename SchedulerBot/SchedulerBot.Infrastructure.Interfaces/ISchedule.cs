using System;

namespace SchedulerBot.Infrastructure.Interfaces
{
	public interface ISchedule
	{
		string Text { get; }

		DateTime NextOccurence { get; }
	}
}
