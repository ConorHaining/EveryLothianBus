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
            var timestamp = (int)DateTimeOffset.Now.ToUnixTimeSeconds();
            timestamp = CreateLocalTimestamp(timestamp);

            return GetAllLatestLocationsAtTimestamp(timestamp);
        }

        public List<VehicleLocation> GetAllLatestLocationsAtTimestamp(int timestamp, bool activeOnly = true)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var busContext = scope.ServiceProvider.GetRequiredService<BusContext>();

                var oneMinuteAgo = (int)DateTimeOffset.FromUnixTimeSeconds(timestamp)
                    .AddMinutes(-1)
                    .ToUnixTimeSeconds();


                var lastMinuteStamps = from x in busContext.VehicleLocations
                                            where (x.ServiceName != null || x.JourneyId != null)
                                                    && x.LastGpsFix >= oneMinuteAgo && 
                                                    x.LastGpsFix <= timestamp
                                            select x;

                var latestTimeStamps = from x in lastMinuteStamps
                                        group x by x.VehicleId into groupedBuses
                                        select new { VehicleId = groupedBuses.Key, LastGpsFix = groupedBuses.Max(y => y.LastGpsFix)};

                var latestRecords = from x in lastMinuteStamps
                                    join latest in latestTimeStamps
                                    on new { x1 = x.VehicleId, x2 = x.LastGpsFix } equals new { x1 = latest.VehicleId, x2 = latest.LastGpsFix }
                                    select x;

               return latestRecords.ToList();
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

        private int CreateLocalTimestamp(int timestamp)
        {
            var ukTimezone = TimeZoneInfo.FindSystemTimeZoneById("Europe/London");
            var datetime = DateTime.UnixEpoch.AddSeconds(timestamp);
            datetime = TimeZoneInfo.ConvertTimeFromUtc(datetime, ukTimezone);
            var offset = new DateTimeOffset(datetime);
            return (int)offset.ToUnixTimeSeconds();
        }
    }
}