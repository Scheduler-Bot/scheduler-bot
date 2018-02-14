using System;
using SchedulerBot.Infrastructure.Interfaces;

namespace SchedulerBot.Infrastructure.Utils
{
	public class Schedule : ISchedule
	{
		public Schedule(string text, DateTime nextOccurence)
		{
			Text = text;
			NextOccurence = nextOccurence;
		}

		public string Text { get; }

		public DateTime NextOccurence { get; }
	}
}
