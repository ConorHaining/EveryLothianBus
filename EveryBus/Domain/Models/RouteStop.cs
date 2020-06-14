using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EveryBus.Domain.Models
{
    public class RouteStop
    {
        public int Order { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RouteId { get; set; }
        public Route Route { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid StopId { get; set; }
        public Stop Stop { get; set; }
    }
}
