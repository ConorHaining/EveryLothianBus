namespace EveryBus.Domain.Models
{
    public class VehicleLocation
    {
        public string Id { get; set; }
        public string VehicleId { get; set; }
        public int LastGpsFix { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int? Speed { get; set; }
        public int? Heading { get; set; }
        public string ServiceName { get; set; }
        public string Destination { get; set; }
        public string JourneyId { get; set; }
        public string NextStopId { get; set; }
        public string VehicleType { get; set; }
    }
}