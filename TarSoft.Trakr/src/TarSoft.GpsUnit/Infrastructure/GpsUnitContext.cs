using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace TarSoft.GpsUnit.Infrastructure
{
    public class GpsUnitContext : DbContext
    {
        public GpsUnitContext(DbContextOptions<GpsUnitContext> options) : base(options){}

        public DbSet<Domain.GpsUnit> GpsUnits { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Automatically apply all configurations
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
