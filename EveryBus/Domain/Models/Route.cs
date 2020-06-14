using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EveryBus.Domain.Models
{
    public class Route
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [JsonPropertyName("destination")]
        public string Destination { get; set; }

        [JsonPropertyName("points")]
        [NotMapped]
        public List<Stop> Stops { get; set; }

        public List<RouteStop> RouteStops { get; set; }
    }
}