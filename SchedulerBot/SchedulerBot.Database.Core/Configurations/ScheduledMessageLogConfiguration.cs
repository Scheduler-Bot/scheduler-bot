using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchedulerBot.Database.Entities;

namespace SchedulerBot.Database.Core.Configurations
{
	public class ScheduledMessageLogConfiguration : IEntityTypeConfiguration<ScheduledMessageLog>
	{
		public void Configure(EntityTypeBuilder<ScheduledMessageLog> builder)
		{
			builder.HasKey(run => run.Id);
			builder.Property(run => run.Id).ValueGeneratedOnAdd();
			builder
				.HasOne(run => run.ScheduledMessage)
				.WithMany(message => message.Runs)
				.HasForeignKey(run => run.ScheduledMessageId);
		}
	}
}
