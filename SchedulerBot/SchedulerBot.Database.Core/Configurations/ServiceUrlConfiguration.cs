using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchedulerBot.Database.Entities;

namespace SchedulerBot.Database.Core.Configurations
{
	/// <summary>
	/// Configures the <see cref="ServiceUrl"/> entity properties.
	/// </summary>
	/// <seealso cref="IEntityTypeConfiguration{T}" />
	public class ServiceUrlConfiguration : IEntityTypeConfiguration<ServiceUrl>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<ServiceUrl> builder)
		{
			builder.HasKey(url => url.Id);
			builder
				.Property(url => url.Id)
				.ValueGeneratedOnAdd();
			builder
				.Property(url => url.Address)
				.IsRequired();
			builder
				.HasIndex(url => url.Address)
				.IsUnique();
		}
	}
}
