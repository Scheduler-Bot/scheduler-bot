using Microsoft.EntityFrameworkCore;
using SchedulerBot.Database.Entities;

namespace SchedulerBot.Database.Core
{
	public class SchedulerBotContext : DbContext
	{
		public SchedulerBotContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<ScheduledMessage> ScheduledMessages { get; set; }
	}
}
