using Microsoft.EntityFrameworkCore;
using SchedulerBot.Database.Core.Configurations;
using SchedulerBot.Database.Entities;

namespace SchedulerBot.Database.Core
{
	/// <summary>
	/// The database context for the scheduled bot application.
	/// </summary>
	/// <seealso cref="DbContext" />
	public class SchedulerBotContext : DbContext
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SchedulerBotContext"/> class.
		/// </summary>
		/// <param name="options">The options for this context.</param>
		public SchedulerBotContext(DbContextOptions options) : base(options)
		{
		}

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

		/// <inheritdoc />
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfiguration(new ScheduledMessageConfiguration());
			modelBuilder.ApplyConfiguration(new ScheduledMessageEventConfiguration());
			modelBuilder.ApplyConfiguration(new ScheduledMessageDetailsConfiguration());
		}
	}
}
