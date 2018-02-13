using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchedulerBot.Database.Entities;

namespace SchedulerBot.Database.Core.Configurations
{
	public class ScheduledMessageRunConfiguration : IEntityTypeConfiguration<ScheduledMessageRun>
	{
		public void Configure(EntityTypeBuilder<ScheduledMessageRun> builder)
		{
			builder.HasKey(run => new { run.ScheduledMessageId, run.RanAt });
			builder
				.HasOne(run => run.ScheduledMessage)
				.WithMany(message => message.Runs)
				.HasForeignKey(run => run.ScheduledMessageId);
		}
	}
}
