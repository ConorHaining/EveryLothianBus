using System.Collections.Generic;
using EveryBus.Domain.Models;

namespace EveryBus.Services.Interfaces
{
    public interface IVehicleLocationsService
    {
        List<VehicleLocation> GetAllLatestLocations();
        VehicleLocation GetSpecificLatestLocation(string VehicleId);
    }
}