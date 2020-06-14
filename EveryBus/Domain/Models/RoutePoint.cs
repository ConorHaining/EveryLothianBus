using EveryBus.Domain.Converters;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace EveryBus.Domain.Models
{
    public class RoutePoint
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [JsonPropertyName("stop_id")]
        [JsonConverter(typeof(StopIdToIntConverter))]
        public int? StopId { get; set; }

        [JsonPropertyName("latitude")]
        [Column(TypeName = "decimal(9, 6)")]
        public decimal Latitude { get; set; }

        [JsonPropertyName("longitude")]
        [Column(TypeName = "decimal(9, 6)")]
        public decimal Longitude { get; set; }

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
