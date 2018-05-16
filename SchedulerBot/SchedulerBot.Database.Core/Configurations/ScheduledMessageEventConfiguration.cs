using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchedulerBot.Database.Entities;

namespace SchedulerBot.Database.Core.Configurations
{
	/// <summary>
	/// Configures the <see cref="ScheduledMessageEvent"/> entity properties.
	/// </summary>
	/// <seealso cref="IEntityTypeConfiguration{T}" />
	public class ScheduledMessageEventConfiguration : IEntityTypeConfiguration<ScheduledMessageEvent>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<ScheduledMessageEvent> builder)
		{
			builder.HasKey(@event => @event.Id);
			builder.Property(@event => @event.Id).ValueGeneratedOnAdd();
			builder
				.HasOne(@event => @event.ScheduledMessage)
				.WithMany(message => message.Events)
				.HasForeignKey(@event => @event.ScheduledMessageId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
