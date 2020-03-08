using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EveryBus.Domain.Models
{
    public class Stop
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [JsonPropertyName("stop_id")]
        public string StopId { get; set; }
        
        [JsonPropertyName("latitude")]
        public decimal Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public decimal Longitude { get; set; }
    }
}