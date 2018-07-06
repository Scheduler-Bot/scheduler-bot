using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchedulerBot.Database.Core.Configurations;
using SchedulerBot.Database.Entities;
using SchedulerBot.Database.Entities.Interfaces;

namespace SchedulerBot.Database.Core
{
	/// <summary>
	/// The database context for the scheduled bot application.
	/// </summary>
	/// <seealso cref="DbContext" />
	public class SchedulerBotContext : DbContext
	{
		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SchedulerBotContext"/> class.
		/// </summary>
		/// <param name="options">The options for this context.</param>
		public SchedulerBotContext(DbContextOptions options) : base(options)
		{
		}

		#endregion

		#region DbSets

		/// <summary>
		/// Gets or sets the manage conversation links.
		/// </summary>
		public DbSet<ManageConversationLink> ManageConversationLinks { get; set; }

		/// <summary>
		/// Gets or sets the scheduled messages.
		/// </summary>
		public DbSet<ScheduledMessage> ScheduledMessages { get; set; }

		/// <summary>
		/// Gets or sets the scheduled message events.
		/// </summary>
		public DbSet<ScheduledMessageEvent> ScheduledMessageEvents { get; set; }

		/// <summary>
		/// Gets or sets the scheduled message details.
		/// </summary>
		public DbSet<ScheduledMessageDetails> ScheduledMessageDetails { get; set; }

		/// <summary>
		/// Gets or sets the scheduled message details service urls.
		/// </summary>
		public DbSet<ScheduledMessageDetailsServiceUrl> ScheduledMessageDetailsServiceUrls { get; set; }

		/// <summary>
		/// Gets or sets the service urls.
		/// </summary>
		public DbSet<ServiceUrl> ServiceUrls { get; set; }

		#endregion

		#region Overrides

		/// <inheritdoc />
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfiguration(new ManageConversationLinkConfiguration());
			modelBuilder.ApplyConfiguration(new ScheduledMessageConfiguration());
			modelBuilder.ApplyConfiguration(new ScheduledMessageEventConfiguration());
			modelBuilder.ApplyConfiguration(new ScheduledMessageDetailsConfiguration());
			modelBuilder.ApplyConfiguration(new ScheduledMessageDetailsServiceUrlConfiguration());
			modelBuilder.ApplyConfiguration(new ServiceUrlConfiguration());
		}

		/// <inheritdoc />
		public override int SaveChanges(bool acceptAllChangesOnSuccess)
		{
			SetCreatedOnValue();

			return base.SaveChanges(acceptAllChangesOnSuccess);
		}

		/// <inheritdoc />
		public override Task<int> SaveChangesAsync(
			bool acceptAllChangesOnSuccess,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			SetCreatedOnValue();

			return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}

		#endregion

		#region Private Methods

		private void SetCreatedOnValue()
		{
			IEnumerable<ICreatedOn> addedEntities = ChangeTracker
				.Entries<ICreatedOn>()
				.Where(entry => entry.State == EntityState.Added)
				.Select(entry => entry.Entity);
			DateTime currentTime = DateTime.UtcNow;

			foreach (ICreatedOn addedEntity in addedEntities)
			{
				addedEntity.CreatedOn = currentTime;
			}
		}

		#endregion
	}
}
