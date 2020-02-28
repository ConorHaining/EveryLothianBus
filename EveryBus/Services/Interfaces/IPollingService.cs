using System;
using EveryBus.Domain.Models;

namespace EveryBus.Services.Interfaces
{
    public interface IPollingService : IObservable<VehicleLocation[]>
    {
        PollingStatus Start();
        PollingStatus Stop();
        PollingStatus Status();
    }
}