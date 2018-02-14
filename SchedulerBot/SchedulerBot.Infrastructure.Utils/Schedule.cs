using System;
using SchedulerBot.Infrastructure.Interfaces;

namespace SchedulerBot.Infrastructure.Utils
{
	public class Schedule : ISchedule
	{
		public Schedule(DateTime nextOccurence)
		{
			NextOccurence = nextOccurence;
		}

		public DateTime NextOccurence { get; }
	}
}
