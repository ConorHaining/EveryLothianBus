using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EveryBus.Domain.Models
{
    public class StopResponse
    {
        [JsonPropertyName("last_updated")]
        public int LastUpdated { get; set; }

        [JsonPropertyName("stops")]
        public List<Stop> Stops { get; set; }
    }
}