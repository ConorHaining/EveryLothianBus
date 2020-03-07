using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EveryBus.Domain.Models
{
    public class Route
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Destination { get; set; }
        public Point[] Points { get; set; }
    }
}