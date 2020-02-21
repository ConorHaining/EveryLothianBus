using System.Text.Json.Serialization;

namespace EveryBus.Domain.Models
{
    public class VehicleLocationResponse
    {
        public int LastUpdated { get; set; }

        public VehicleLocation[] vehicleLocations { get; set; }
    }
}