using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchedulerBot.Database.Entities;

namespace SchedulerBot.Database.Core.Configurations
{
	public class ScheduledMessageRunConfiguration : IEntityTypeConfiguration<ScheduledMessageLog>
	{
		public void Configure(EntityTypeBuilder<ScheduledMessageLog> builder)
		{
			builder.HasKey(run => new { run.ScheduledMessageId, RanAt = run.CreatedOn });
			builder
				.HasOne(run => run.ScheduledMessage)
				.WithMany(message => message.Runs)
				.HasForeignKey(run => run.ScheduledMessageId);
		}
	}
}
