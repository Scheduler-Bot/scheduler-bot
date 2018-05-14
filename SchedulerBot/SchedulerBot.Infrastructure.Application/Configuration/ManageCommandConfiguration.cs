using System;
using SchedulerBot.Infrastructure.Interfaces.Configuration;

namespace SchedulerBot.Infrastructure.Application.Configuration
{
	public class ManageCommandConfiguration : IManageCommandConfiguration
	{
		public TimeSpan LinkExpirationPeriod { get; set; }

		public int LinkIdLength { get; set; }
	}
}
