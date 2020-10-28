using EveryBus.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EveryBus.Domain
{
    public class BusContext : DbContext
    {
        public BusContext(DbContextOptions<BusContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VehicleLocation>()
                .HasKey(x => new { x.Id, x.ReportTime });

            modelBuilder.Entity<RouteStop>()
                .HasKey(x => new { x.Order, x.RouteId, x.StopId });

            modelBuilder.Entity<RouteStop>()
                .HasOne(x => x.Stop)
                .WithMany(x => x.RouteStops)
                .HasForeignKey(x => x.StopId);

            modelBuilder.Entity<RouteStop>()
                .HasOne(x => x.Route)
                .WithMany(x => x.RouteStops)
                .HasForeignKey(x => x.RouteId);
        }

        public DbSet<Stop> Stops { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<VehicleLocation> VehicleLocations { get; set; }
        public DbSet<RouteStop> RouteStop { get; set; }
    }
}