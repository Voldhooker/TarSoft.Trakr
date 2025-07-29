using Microsoft.EntityFrameworkCore;
using TelemetryIngestor.Domain;

namespace TelemetryIngestor.Infrastructure;

public class TelemetryDbContext : DbContext
{
    public TelemetryDbContext(DbContextOptions<TelemetryDbContext> options) : base(options) { }

    public DbSet<TelemetryData> TelemetryData { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TelemetryData>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DeviceId).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Latitude).IsRequired();
            entity.Property(e => e.Longitude).IsRequired();
            entity.Property(e => e.Timestamp).IsRequired();
            entity.Property(e => e.ReceivedAt).IsRequired();
            entity.Property(e => e.AdditionalData).HasMaxLength(2000);
            
            entity.HasIndex(e => new { e.DeviceId, e.Timestamp });
            entity.HasIndex(e => e.ReceivedAt);
        });

        base.OnModelCreating(modelBuilder);
    }
}
