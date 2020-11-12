using EveryBus.Domain;
using EveryBus.Domain.Models;
using EveryBus.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EveryBus.Services
{
    public class VehicleLocationsService : IVehicleLocationsService
    {
        private readonly BusContext _busContext;
        private readonly IMemoryCache cache;

        public VehicleLocationsService(BusContext busContext, IMemoryCache cache)
        {
            _busContext = busContext;
            this.cache = cache;
        }

        public List<VehicleLocation> GetAllLatestLocations(bool activeOnly = true)
        {
            //return cache.GetOrCreate("vehicles", updates =>
            //{
            return GetAllLatestLocationsAtTimestamp(DateTime.Now);
            //});
        }

        public List<VehicleLocation> GetAllLatestLocationsAtTimestamp(DateTime timestamp, bool activeOnly = true)
        {
            var fiveMintuesAgo = timestamp.AddMinutes(-5);

            FormattableString query = $"SELECT DISTINCT ON(\"VehicleId\") * FROM \"VehicleLocations\" WHERE \"ReportTime\" BETWEEN CAST('{fiveMintuesAgo}' AS TIMESTAMP) AND CAST('{timestamp}' AS TIMESTAMP) ORDER BY \"VehicleId\", \"ReportTime\" DESC";

            return _busContext.VehicleLocations.FromSqlRaw(query.ToString()).ToList();
        }

        public VehicleLocation GetSpecificLatestLocation(string VehicleId)
        {


            var lastestReports = _busContext.VehicleLocations
                                    .Where(x => x.VehicleId == VehicleId)
                                    .OrderByDescending(x => x.ReportTime)
                                    .First();

            return lastestReports;

        }
    }
}