using System;
using SchedulerBot.Infrastructure.Interfaces.Configuration;

namespace SchedulerBot.Infrastructure.Application.Configuration
{
	/// <inheritdoc />
	public class ManageCommandConfiguration : IManageCommandConfiguration
	{
		/// <inheritdoc />

		public TimeSpan LinkExpirationPeriod { get; set; }

		/// <inheritdoc />
		public int LinkIdLength { get; set; }
	}
}
