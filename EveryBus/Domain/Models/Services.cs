using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EveryBus.Domain.Models
{
    public class BusServices
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public string Name;

        public string Description { get; set; }

        public string ServiceType { get; set; }

        public Route[] Routes { get; set; }
    }
}