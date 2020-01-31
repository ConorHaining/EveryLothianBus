namespace EveryBus.Models
{
    public class VehicleLocationsResponse
    {
        public double last_updated { get; set; }
        public VehicleLocation[] vehicles { get; set; }
    }

}