using System.ComponentModel.DataAnnotations.Schema;

namespace EveryBus.Domain.Models
{
    public class Point
    {
        public string Id { get; set; }
        public int StopId { get; set; }
        public long Latitude { get; set; }
        public long Longitude { get; set; }
    }
}