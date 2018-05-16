using System;

namespace SchedulerBot.Database.Entities.Interfaces
{
	/// <summary>
	/// Provides the information about the instance creation time.
	/// </summary>
	public interface ICreatedOn
	{
		/// <summary>
		/// Gets or sets the instance creation time.
		/// </summary>
		DateTime CreatedOn { get; set; }
	}
}
