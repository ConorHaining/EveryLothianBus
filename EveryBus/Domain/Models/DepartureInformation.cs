using System.Text.Json.Serialization;

namespace EveryBus.Domain.Models
{
    public class DepartureInformation
    {
        [JsonPropertyName("routeName")]
        public string RouteName { get; set; }

        [JsonPropertyName("routeColour")]
        public string RouteColour { get; set; }

        [JsonPropertyName("textColour")]
        public string TextColour { get; set; }

        [JsonPropertyName("departures")]
        public Departure[] Departures { get; set; }
    }
}
