using System.ComponentModel.DataAnnotations.Schema;

namespace EveryBus.Domain.Models
{
    public class Route
    {
        public string Id { get; set; }
        public string Destination { get; set; }
        public Point[] Points { get; set; }
    }
}