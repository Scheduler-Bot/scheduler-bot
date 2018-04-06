using System;
using System.Collections.Generic;
using SchedulerBot.Database.Entities.Interfaces;

namespace SchedulerBot.Database.Entities
{
	/// <summary>
	/// Holds the information about a service URL used to deliver messages to a client.
	/// </summary>
	public class ServiceUrl : ICreatedOn
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets the service URL address.
		/// </summary>
		public string Address { get; set; }

		/// <summary>
		/// Gets or sets the entity creation time.
		/// </summary>
		public DateTime CreatedOn { get; set; }

		/// <summary>
		/// Gets or sets the collection of <see cref="ScheduledMessageDetailsServiceUrl"/> objects
		/// connecting the current instance to corresponding <see cref="ScheduledMessageDetails"/> objects.
		/// </summary>
		public ICollection<ScheduledMessageDetailsServiceUrl> ServiceUrlMessageDetails { get; set; }
	}
}
