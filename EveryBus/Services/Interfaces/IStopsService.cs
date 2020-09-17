using EveryBus.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EveryBus.Services.Interfaces
{
    public interface IStopsService
    {
        public Stop GetByAtocCode(string AtocCode);

        public Stop GetByStopId(int StopId);

        public IEnumerable<Route> GetRoutesAtStop(int StopId);
    }
}
