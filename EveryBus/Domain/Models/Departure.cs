using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EveryBus.Domain.Models
{
    public class Departure
    {
        [JsonPropertyName("sms")]
        public string StopId { get; set; }

        [JsonPropertyName("routeName")]
        public string RouteName { get; set; }

        [JsonPropertyName("destination")]
        public string Destination { get; set; }

        [JsonPropertyName("tripId")]
        public string TripId { get; set; }

        [JsonPropertyName("vehicleId")]
        public string VehicleId { get; set; }

        [JsonPropertyName("departureTime")]
        public string DepartureTime { get; set; }

        [JsonPropertyName("isLive")]
        public bool IsLive { get; set; }

        [JsonPropertyName("isDiverted")]
        public bool IsDiverted { get; set; }
    }
}
