using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchedulerBot.Database.Entities;

namespace SchedulerBot.Database.Core.Configurations
{
	public class ScheduledMessageDetailsConfiguration : IEntityTypeConfiguration<ScheduledMessageDetails>
	{
		public void Configure(EntityTypeBuilder<ScheduledMessageDetails> builder)
		{
			builder.HasKey(details => details.ScheduledMessageId);
			builder
				.HasOne(details => details.ScheduledMessage)
				.WithOne(message => message.Details)
				.HasForeignKey<ScheduledMessageDetails>(details => details.ScheduledMessageId);
			builder.Property(details => details.FromId).IsRequired();
			builder.Property(details => details.FromName).IsRequired();
			builder.Property(details => details.RecipientId).IsRequired();
			builder.Property(details => details.RecipientName).IsRequired();
			builder.Property(details => details.ChannelId).IsRequired();
			builder.Property(details => details.ConversationId).IsRequired();
			builder.Property(details => details.Locale).IsRequired();
		}
	}
}
