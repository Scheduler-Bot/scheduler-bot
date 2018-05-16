using System;
using SchedulerBot.Database.Entities.Interfaces;

namespace SchedulerBot.Database.Entities
{
	/// <summary>
	/// Enables many-to-many relationship between <see cref="ScheduledMessageDetails"/> and <see cref="ServiceUrl"/>.
	/// </summary>
	public class ScheduledMessageDetailsServiceUrl : ICreatedOn
	{
		/// <summary>
		/// Gets or sets the scheduled message details identifier.
		/// </summary>
		public Guid DetailsId { get; set; }

		/// <summary>
		/// Gets or sets the scheduled message details.
		/// </summary>
		public ScheduledMessageDetails Details { get; set; }

		/// <summary>
		/// Gets or sets the service URL identifier.
		/// </summary>
		public Guid ServiceUrlId { get; set; }

		/// <summary>
		/// Gets or sets the service URL.
		/// </summary>
		public ServiceUrl ServiceUrl { get; set; }

		/// <summary>
		/// Gets or sets the entity creation time.
		/// </summary>
		public DateTime CreatedOn { get; set; }
	}
}
