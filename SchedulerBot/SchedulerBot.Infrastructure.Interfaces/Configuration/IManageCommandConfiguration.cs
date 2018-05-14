using System;

namespace SchedulerBot.Infrastructure.Interfaces.Configuration
{
	public interface IManageCommandConfiguration
	{
		TimeSpan LinkExpirationPeriod { get; set; }

		int LinkIdLength { get; set; }
	}
}
