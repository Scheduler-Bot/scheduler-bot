﻿using System;
using System.Collections.Generic;

namespace SchedulerBot.Infrastructure.Interfaces.Schedule
{
	public interface ISchedule
	{
		string Text { get; }

		DateTime GetNextOccurence();

		DateTime GetNextOccurence(DateTime baseTime);

		IEnumerable<DateTime> GetNextOccurences(DateTime baseTime, DateTime endTime);
	}
}
