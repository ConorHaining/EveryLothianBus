using System;
using System.Collections.Generic;
using EveryBus.Domain.Models;

namespace EveryBus.Services.Interfaces
{
    public interface IVehicleLocationsService
    {
        List<VehicleLocation> GetAllLatestLocations(bool activeOnly = true);
        List<VehicleLocation> GetAllLatestLocationsAtTimestamp(DateTime timestamp, bool activeOnly = true);
        VehicleLocation GetSpecificLatestLocation(string VehicleId);
    }
}