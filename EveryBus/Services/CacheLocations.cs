using EveryBus.Domain.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace EveryBus.Services
{
    public class CacheLocations : IObserver<List<VehicleLocation>>
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

        public void OnNext(List<VehicleLocation> vehicleUpdates)
        {
            if (vehicleUpdates == null)
            {
                return;
            } 
            else
            {
                cache.Set($"vehicles", vehicleUpdates, TimeSpan.FromSeconds(60));
            }
        }
    }
}
