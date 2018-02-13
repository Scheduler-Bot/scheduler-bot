using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchedulerBot.Database.Entities;

namespace SchedulerBot.Database.Core.Configurations
{
	public class ScheduledMessageLogConfiguration : IEntityTypeConfiguration<ScheduledMessageLog>
	{
		public void Configure(EntityTypeBuilder<ScheduledMessageLog> builder)
		{
			builder.HasKey(log => log.Id);
			builder.Property(log => log.Id).ValueGeneratedOnAdd();
			builder
				.HasOne(log => log.ScheduledMessage)
				.WithMany(message => message.Logs)
				.HasForeignKey(log => log.ScheduledMessageId);
		}
	}
}
