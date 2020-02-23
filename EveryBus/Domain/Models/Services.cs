namespace EveryBus.Domain.Models
{
    public class BusServices
    {
        public string Id { get; set; }
        
        public string Name;

        public string Description { get; set; }

        public string ServiceType { get; set; }

        public Route[] Routes { get; set; }
    }
}