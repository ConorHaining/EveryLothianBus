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
            using (var scope = _scopeFactory.CreateScope())
            {
                var busContext = scope.ServiceProvider.GetRequiredService<BusContext>();

                Func<VehicleLocation, bool> activeOnlyClause = x => true;
                if (activeOnly) {
                    activeOnlyClause = x => x.ServiceName != null || x.JourneyId != null;
                }

                var lastestReports = busContext.VehicleLocations
                                        .Where(x => x.ServiceName != null || x.JourneyId != null)
                                        .GroupBy(x => x.VehicleId)
                                        .Select(x => new { VehicleId = x.Key, LastestReport = x.Max(x => x.LastGpsFix) })
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