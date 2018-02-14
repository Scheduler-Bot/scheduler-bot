using System;

namespace SchedulerBot.Infrastructure.Interfaces
{
	public interface ISchedule
	{
		DateTime NextOccurence { get; }
	}
}
