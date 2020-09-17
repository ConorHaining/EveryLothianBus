using EveryBus.Domain;
using EveryBus.Domain.Models;
using EveryBus.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EveryBus.Services
{
    public class StopsService : IStopsService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public StopsService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Stop GetByAtocCode(string AtocCode)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var busContext = scope.ServiceProvider.GetRequiredService<BusContext>();

                return busContext.Stops.FirstOrDefault(x => x.AtocCode == AtocCode);
            }
        }

        public Stop GetByStopId(int StopId)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var busContext = scope.ServiceProvider.GetRequiredService<BusContext>();

                return busContext.Stops.FirstOrDefault(x => x.StopId == StopId);
            }
        }

        public IEnumerable<Route> GetRoutesAtStop(int StopId)
        {
            throw new NotImplementedException();
        }
    }
}