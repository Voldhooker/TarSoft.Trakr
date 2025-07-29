using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace TarSoft.GpsUnit.Infrastructure.EntityConfiguration
{
    public class GpsUnitConfiguration : IEntityTypeConfiguration<Domain.GpsUnit>
    {
        public void Configure(EntityTypeBuilder<Domain.GpsUnit> builder)
        {
            // Since GpsUnit inherits from Entity, it automatically includes the configurations from EntityConfiguration
            builder.Property(g => g.ApiKey).IsRequired();
            builder.Property(g => g.Name).IsRequired().HasMaxLength(255);
            builder.Property(g => g.Description).IsRequired(false);
        }
    }
}
