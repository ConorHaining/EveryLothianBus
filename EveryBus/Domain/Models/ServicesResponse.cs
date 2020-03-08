using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EveryBus.Domain.Models
{
    public class ServicesResponse
    {
        [JsonPropertyName("last_updated")]
        public int LastUpdated { get; set; }

        [JsonPropertyName("services")]
        public List<BusServices> services { get; set; }
    }
}