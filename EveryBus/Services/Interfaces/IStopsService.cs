using EveryBus.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EveryBus.Services.Interfaces
{
    public interface IStopsService
    {
        Stop GetByStopId(int StopId);
        Stop GetByAtocCode(string StopId);
    }
}
