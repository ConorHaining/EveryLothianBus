using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using EveryBus.Domain.Converters;

namespace EveryBus.Domain.Models
{
    public class Stop : IEquatable<Stop>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [JsonPropertyName("stop_id")]
        [JsonConverter(typeof(StopIdToIntConverter))]
        public int? StopId { get; set; }

        [JsonPropertyName("atco_code")]
        public string AtocCode { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("identifier")]
        public string Identifier { get; set; }

        [JsonPropertyName("locality")]
        public string Locality { get; set; }

        [JsonPropertyName("orientation")]
        public int Orientation { get; set; }

        [JsonPropertyName("direction")]
        public string Direction { get; set; }

        [JsonPropertyName("latitude")]
        [Column(TypeName = "decimal(9, 6)")]
        public decimal Latitude { get; set; }

        [JsonPropertyName("longitude")]
        [Column(TypeName = "decimal(9, 6)")]
        public decimal Longitude { get; set; }

        [JsonPropertyName("service_type")]
        public string ServiceType { get; set; }
        public List<RouteStop> RouteStops { get; set; }

        public bool Equals([AllowNull] Stop other)
        {
            if (other is null)
            {
                return false;
            }

            return Decimal.Round(this.Latitude, 6) == Decimal.Round(other.Latitude, 6)
                   && Decimal.Round(this.Longitude, 6) == Decimal.Round(other.Longitude, 6)
                   && this.StopId == other.StopId;
        }

        public override bool Equals(object obj) => Equals(obj as Stop);
        public override int GetHashCode() => (StopId, Latitude, Longitude).GetHashCode();
    }
}