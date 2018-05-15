using System;

namespace SchedulerBot.Infrastructure.Interfaces.Configuration
{
	/// <summary>
	/// Represents the manage command configuration.
	/// </summary>
	public interface IManageCommandConfiguration
	{
		/// <summary>
		/// Gets or sets the manage link expiration period.
		/// </summary>
		TimeSpan LinkExpirationPeriod { get; set; }

		/// <summary>
		/// Gets or sets the length of the link identifier.
		/// </summary>
		int LinkIdLength { get; set; }
	}
}
