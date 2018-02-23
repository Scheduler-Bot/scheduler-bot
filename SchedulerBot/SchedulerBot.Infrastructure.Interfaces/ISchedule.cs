using System;

namespace SchedulerBot.Infrastructure.Interfaces
{
	public interface ISchedule
	{
		string Text { get; }

		DateTime GetNextOccurence();

		DateTime GetNextOccurence(DateTime baseTime);
	}
}
