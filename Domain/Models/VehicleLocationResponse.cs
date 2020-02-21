using System.Text.Json.Serialization;

namespace EveryBus.Domain.Models
{
    public class VehicleLocationResponse
    {
        [JsonPropertyName("last_updated")]
        public int LastUpdated { get; set; }

        [JsonPropertyName("vehicles")]
        public VehicleLocation[] vehicleLocations { get; set; }
    }
}