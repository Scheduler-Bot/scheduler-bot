using Microsoft.EntityFrameworkCore;
using SchedulerBot.Database.Core.Configurations;
using SchedulerBot.Database.Entities;

namespace SchedulerBot.Database.Core
{
	public class SchedulerBotContext : DbContext
	{
		public SchedulerBotContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<ScheduledMessage> ScheduledMessages { get; set; }

		public DbSet<ScheduledMessageLog> ScheduledMessageLogs { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfiguration(new ScheduledMessageConfiguration());
			modelBuilder.ApplyConfiguration(new ScheduledMessageLogConfiguration());
		}
	}
}
