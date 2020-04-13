using System.Collections.Generic;
using EveryBus.Domain.Models;

namespace EveryBus.Services.Interfaces
{
    public interface IRouteService
    {
        void CreateRoutes();

        IEnumerable<Service> GetRoutes();
     }
}