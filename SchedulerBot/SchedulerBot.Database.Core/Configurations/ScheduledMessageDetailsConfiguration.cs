using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchedulerBot.Database.Entities;

namespace SchedulerBot.Database.Core.Configurations
{
	/// <summary>
	/// Configures the <see cref="ScheduledMessageDetails"/> entity properties.
	/// </summary>
	/// <seealso cref="IEntityTypeConfiguration{T}" />
	public class ScheduledMessageDetailsConfiguration : IEntityTypeConfiguration<ScheduledMessageDetails>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<ScheduledMessageDetails> builder)
		{
			builder.HasKey(details => details.ScheduledMessageId);
			builder
				.HasOne(details => details.ScheduledMessage)
				.WithOne(message => message.Details)
				.HasForeignKey<ScheduledMessageDetails>(details => details.ScheduledMessageId)
				.OnDelete(DeleteBehavior.Restrict);
			builder.Property(details => details.FromId).IsRequired();
			builder.Property(details => details.FromName).IsRequired();
			builder.Property(details => details.RecipientId).IsRequired();
			builder.Property(details => details.RecipientName).IsRequired();
			builder.Property(details => details.ChannelId).IsRequired();
			builder.Property(details => details.ConversationId).IsRequired();
			builder.Property(details => details.Locale).IsRequired(false);
			builder.Property(details => details.TimeZoneOffset).IsRequired(false);
		}
	}
}
