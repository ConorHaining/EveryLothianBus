using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EveryBus.Domain.Models
{
    public class BusServices
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("service_type")]
        public string ServiceType { get; set; }
        
        [JsonPropertyName("routes")]    
        public List<Route> Routes { get; set; }
    }
}