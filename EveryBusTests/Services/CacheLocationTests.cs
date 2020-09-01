using EveryBus.Domain.Models;
using EveryBus.Services;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;
using NUnit.Framework;
using Shouldly;

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
            VehicleLocation[] vehicleUpdates = null;

            cacheLocations.OnNext(vehicleUpdates);
        }

        [Test]
        public void OnNext_ShouldAddToCache_WhenGivenUpdates()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var cacheLocations = new CacheLocations(cache);
            VehicleLocation[] vehicleUpdates = new VehicleLocation[]
            {
                new VehicleLocation(),
            };

            cacheLocations.OnNext(vehicleUpdates);

            cache.Count.ShouldBeGreaterThan(0);
        }

        [Test]
        public void OnNext_ShouldAddToCacheForEachGivenValue_WhenUnqiueVehicleId()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var cacheLocations = new CacheLocations(cache);
            VehicleLocation[] vehicleUpdates = new VehicleLocation[]
            {
                new VehicleLocation(){ VehicleId = "123" },
                new VehicleLocation(){ VehicleId = "456" },
                new VehicleLocation(){ VehicleId = "789" },
            };

            cacheLocations.OnNext(vehicleUpdates);

            cache.Count.ShouldBe(3);
        }

        [Test]
        public void OnNext_ShouldUseVehicleIdAsCacheKey()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var cacheLocations = new CacheLocations(cache);
            VehicleLocation[] vehicleUpdates = new VehicleLocation[]
            {
                new VehicleLocation(){ VehicleId = "123" },
                new VehicleLocation(){ VehicleId = "456" },
                new VehicleLocation(){ VehicleId = "789" },
            };

            cacheLocations.OnNext(vehicleUpdates);

            cache.ShouldSatisfyAllConditions(
                () => cache.TryGetValue("vehicle:123", out var valueOne).ShouldBeTrue(),
                () => cache.TryGetValue("vehicle:456", out var valueTwo).ShouldBeTrue(),
                () => cache.TryGetValue("vehicle:789", out var valueThree).ShouldBeTrue()
            );
        }
    }
}
