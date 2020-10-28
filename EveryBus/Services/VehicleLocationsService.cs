using System;
using System.Collections.Generic;
using System.Linq;
using EveryBus.Domain;
using EveryBus.Domain.Models;
using EveryBus.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace EveryBus.Services
{
    public class VehicleLocationsService : IVehicleLocationsService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMemoryCache cache;

        public VehicleLocationsService(IServiceScopeFactory scopeFactory, IMemoryCache cache)
        {
            _scopeFactory = scopeFactory;
            this.cache = cache;
        }

        public List<VehicleLocation> GetAllLatestLocations(bool activeOnly = true)
        {
            return cache.GetOrCreate("vehicles", updates =>
            {
                return GetAllLatestLocationsAtTimestamp(DateTimeOffset.Now);
            });
        }

        public List<VehicleLocation> GetAllLatestLocationsAtTimestamp(DateTimeOffset timestamp, bool activeOnly = true)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var busContext = scope.ServiceProvider.GetRequiredService<BusContext>();

                var oneMinuteAgo = timestamp.AddMinutes(-1);


                var lastMinuteStamps = from x in busContext.VehicleLocations
                                            where (x.ServiceName != null || x.JourneyId != null)
                                                    && x.ReportTime >= oneMinuteAgo && 
                                                    x.ReportTime <= timestamp
                                            select x;

                var latestTimeStamps = from x in lastMinuteStamps
                                        group x by x.VehicleId into groupedBuses
                                        select new { VehicleId = groupedBuses.Key, ReportTime = groupedBuses.Max(y => y.ReportTime) };

                var latestRecords = from x in lastMinuteStamps
                                    join latest in latestTimeStamps
                                    on new { x1 = x.VehicleId, x2 = x.ReportTime } equals new { x1 = latest.VehicleId, x2 = latest.ReportTime }
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
                                        .OrderByDescending(x => x.ReportTime)
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