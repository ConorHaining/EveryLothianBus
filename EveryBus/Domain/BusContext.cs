using EveryBus.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EveryBus.Domain
{
    public class BusContext : DbContext
    {
        public BusContext(DbContextOptions<BusContext> options) : base(options)
        {
        }

        public DbSet<Stop> Stops { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<BusServices> Services { get; set; }
        public DbSet<VehicleLocation> VehicleLocations { get; set; }
    }
}