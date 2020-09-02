using EveryBus.Domain.Models;
using EveryBus.Services;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;

namespace EveryBus.Tests.Services
{
    [TestFixture]
    class CacheLocationTests
    {

        [Test]
        public void OnNext_ShouldReturn_WhenNoGivenUpdates()
        {
            var cache = Substitute.For<IMemoryCache>();
            var cacheLocations = new CacheLocations(cache);
            List<VehicleLocation> vehicleUpdates = null;

            cacheLocations.OnNext(vehicleUpdates);
        }

        [Test]
        public void OnNext_ShouldAddToCache_WhenGivenUpdates()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var cacheLocations = new CacheLocations(cache);
            List<VehicleLocation> vehicleUpdates = new List<VehicleLocation>
            {
                new VehicleLocation(),
            };

            cacheLocations.OnNext(vehicleUpdates);

            cache.Count.ShouldBe(1);
        }

        [Test]
        public void OnNext_ShouldOnlyHaveOneKey_WhenGivenMutlipleUpdates()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var cacheLocations = new CacheLocations(cache);
            List<VehicleLocation> vehicleUpdates = new List<VehicleLocation>
            {
                new VehicleLocation(){ VehicleId = "123" },
                new VehicleLocation(){ VehicleId = "456" },
                new VehicleLocation(){ VehicleId = "789" },
            };

            cacheLocations.OnNext(vehicleUpdates);

            cache.Count.ShouldBe(1);
        }

        [Test]
        public void OnNext_ShouldUseVehicleCacheKey()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var cacheLocations = new CacheLocations(cache);
            List<VehicleLocation> vehicleUpdates = new List<VehicleLocation>
            {
                new VehicleLocation(){ VehicleId = "123" },
                new VehicleLocation(){ VehicleId = "456" },
                new VehicleLocation(){ VehicleId = "789" },
            };

            cacheLocations.OnNext(vehicleUpdates);
            cache.TryGetValue("vehicles", out _).ShouldBeTrue();
        }
    }
}
