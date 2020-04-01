using System;
using System.Collections.Generic;
using System.Linq;
using EveryBus.Domain;
using EveryBus.Domain.Models;
using EveryBus.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EveryBus.Services
{
    public class VehicleLocationsService : IVehicleLocationsService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public VehicleLocationsService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public List<VehicleLocation> GetAllLatestLocations(bool activeOnly = true)
        {
            var timestamp = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            return GetAllLatestLocationsAtTimestamp(timestamp);
        }

        public List<VehicleLocation> GetAllLatestLocationsAtTimestamp(int timestamp, bool activeOnly = true)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var busContext = scope.ServiceProvider.GetRequiredService<BusContext>();

                var fiveMinutesAgo = DateTimeOffset.FromUnixTimeSeconds(timestamp)
                    .AddMinutes(-5)
                    .ToUnixTimeSeconds();

                var lastestReports = busContext.VehicleLocations
                                        .Where(x => x.ServiceName != null || x.JourneyId != null)
                                        .Where(x => x.LastGpsFix >= fiveMinutesAgo && x.LastGpsFix <= timestamp)
                                        .GroupBy(x => x.VehicleId)
                                        .Select(x => new { VehicleId = x.Key, LastestReport = x.Max(x => x.LastGpsFix) })
                                        // .Where(x => x.LastestReport <= fiveMinutesAgo)
                                        .AsEnumerable();

                var result = from locations in busContext.VehicleLocations
                            join latest in lastestReports on
                            new { x1 = locations.VehicleId, x2 = locations.LastGpsFix } equals new { x1 = latest.VehicleId, x2 = latest.LastestReport }
                            select locations;
               return result.ToList();
            }
        }

        public VehicleLocation GetSpecificLatestLocation(string VehicleId)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var busContext = scope.ServiceProvider.GetRequiredService<BusContext>();

                var lastestReports = busContext.VehicleLocations
                                        .Where(x => x.VehicleId == VehicleId)
                                        .OrderByDescending(x => x.LastGpsFix)
                                        .First();

                return lastestReports;
            }
        }
    }
}