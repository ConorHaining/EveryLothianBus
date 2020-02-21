using EveryBus.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EveryBus.Domain
{
    public class BusContext : DbContext
    {
        public DbSet<Point> Points { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<BusServices> Services { get; set; }
        public DbSet<VehicleLocation> VehicleLocations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=buses.db");
    }
}