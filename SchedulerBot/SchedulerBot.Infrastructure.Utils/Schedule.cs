using System;
using SchedulerBot.Infrastructure.Interfaces;

namespace SchedulerBot.Infrastructure.Utils
{
	public class Schedule : ISchedule
	{
		public Schedule(string text, DateTime nextOccurence, TimeSpan? timeZoneOffset)
		{
			Text = text;
			NextOccurence = nextOccurence;
			TimeZoneOffset = timeZoneOffset;
		}

		public string Text { get; }

		public DateTime NextOccurence { get; }

		public TimeSpan? TimeZoneOffset { get; }
	}
}
