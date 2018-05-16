using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchedulerBot.Database.Entities;

namespace SchedulerBot.Database.Core.Configurations
{
	/// <summary>
	/// Configures the <see cref="ScheduledMessageDetailsServiceUrl"/> entity properties.
	/// </summary>
	/// <seealso cref="IEntityTypeConfiguration{T}" />
	public class ScheduledMessageDetailsServiceUrlConfiguration : IEntityTypeConfiguration<ScheduledMessageDetailsServiceUrl>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<ScheduledMessageDetailsServiceUrl> builder)
		{
			builder.HasKey(entity => new { entity.DetailsId, entity.ServiceUrlId });
			builder
				.HasOne(entity => entity.Details)
				.WithMany(details => details.DetailsServiceUrls)
				.HasForeignKey(entity => entity.DetailsId)
				.OnDelete(DeleteBehavior.Restrict);
			builder
				.HasOne(entity => entity.ServiceUrl)
				.WithMany(url => url.ServiceUrlMessageDetails)
				.HasForeignKey(entity => entity.ServiceUrlId)
				.OnDelete(DeleteBehavior.Restrict);
			builder.HasIndex(entity => new { entity.DetailsId, entity.ServiceUrlId, entity.CreatedOn });
		}
	}
}
