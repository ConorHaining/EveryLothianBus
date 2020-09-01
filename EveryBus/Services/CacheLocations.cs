using EveryBus.Domain.Models;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace EveryBus.Services
{
    public class CacheLocations : IObserver<VehicleLocation[]>
    {
        private readonly IMemoryCache cache;

        public CacheLocations(IMemoryCache cache)
        {
            this.cache = cache;
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(VehicleLocation[] vehicleUpdates)
        {
            if (vehicleUpdates == null)
            {
                return;
            } 
            else
            {
                foreach (var update in vehicleUpdates)
                {
                    cache.Set($"vehicle:{update.VehicleId}", update, TimeSpan.FromSeconds(60));
                }
            }
        }
    }
}
