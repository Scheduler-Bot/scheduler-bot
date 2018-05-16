using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchedulerBot.Database.Entities;

namespace SchedulerBot.Database.Core.Configurations
{
	/// <summary>
	/// Configures the <see cref="ManageConversationLinkConfiguration"/> entity properties.
	/// </summary>
	/// <seealso cref="IEntityTypeConfiguration{T}" />
	public class ManageConversationLinkConfiguration : IEntityTypeConfiguration<ManageConversationLink>
	{
		public void Configure(EntityTypeBuilder<ManageConversationLink> builder)
		{
			builder.HasKey(link => link.Id);
			builder
				.Property(link => link.Id)
				.ValueGeneratedOnAdd();
			builder
				.Property(link => link.ChannelId)
				.IsRequired();
			builder
				.Property(link => link.ConversationId)
				.IsRequired();
			builder
				.Property(link => link.Text)
				.HasMaxLength(64)
				.IsRequired();
			builder
				.Property(link => link.IsVisited)
				.IsRequired();
			builder
				.Property(link => link.CreatedOn)
				.IsRequired();
			builder
				.Property(link => link.ExpiresOn)
				.IsRequired();
		}
	}
}
