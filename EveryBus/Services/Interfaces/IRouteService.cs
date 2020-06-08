using System.Collections.Generic;
using EveryBus.Domain.Models;

namespace EveryBus.Services.Interfaces
{
    public interface IRouteService
    {
        IEnumerable<Service> GetRoutes();
     }
}