using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TarSoft.GpsUnit.Domain;

namespace TarSoft.GpsUnit.Infrastructure.EntityConfiguration
{
    // Configuration for the base Entity class
    public class EntityConfiguration : IEntityTypeConfiguration<Entity>
    {
        public void Configure(EntityTypeBuilder<Entity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.CreatedAtUtc).IsRequired();
            builder.Property(e => e.UpdatedAt).IsRequired(false);
            builder.UseTpcMappingStrategy();

            // Configure other common properties and behaviors here

        }
    }
}