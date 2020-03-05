using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EveryBus.Domain.Models
{
    public class VehicleLocation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [JsonPropertyName("vehicle_id")]
        public string VehicleId { get; set; }

        [JsonPropertyName("last_gps_fix")]
        public int LastGpsFix { get; set; }

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("speed")]
        public int? Speed { get; set; }

        [JsonPropertyName("heading")]
        public int? Heading { get; set; }

        [JsonPropertyName("service_name")]
        public string ServiceName { get; set; }

        [JsonPropertyName("destination")]
        public string Destination { get; set; }

        [JsonPropertyName("journey_id")]
        public string JourneyId { get; set; }

        [JsonPropertyName("next_stop_id")]
        public string NextStopId { get; set; }

        [JsonPropertyName("vehicle_type")]
        public string VehicleType { get; set; }
    }
}