using System;

namespace SchedulerBot.Infrastructure.Interfaces.Schedule
{
	public interface ISchedule
	{
		string Text { get; }

		DateTime NextOccurence { get; }
	}
}
