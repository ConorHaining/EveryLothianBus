using EveryBus.DependencyInjection;
using EveryBus.Domain.Models;
using EveryBus.Services;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace EveryBus.Tests.DependencyInjection
{

    [TestFixture]
    public class ObserversTests
    {
        private ServiceCollection services;

        [SetUp]
        public void SetUp()
        {
            services = new ServiceCollection();
        }

        [Test]
        public void AddObservers_ShouldAddPersistLocations()
        {
            services.AddObservers();

            services.ShouldContain(x => x.ImplementationType == typeof(PersistLocations));
        }

        [Test]
        public void AddObservers_ShouldAddPersistLocations_AsAnObserver()
        {
            services.AddObservers();

            services.ShouldContain(x => x.ImplementationType == typeof(PersistLocations) && x.ServiceType == typeof(IObserver<List<VehicleLocation>>));
        }

        [Test]
        public void AddObservers_ShouldAddPersistLocations_AsAScopeService()
        {
            services.AddObservers();
            
            services.ShouldContain(x => x.ImplementationType == typeof(PersistLocations) && x.Lifetime == ServiceLifetime.Scoped);
        }

        [Test]
        public void AddObservers_ShouldAddBroadcastLocations()
        {
            services.AddObservers();

            services.ShouldContain(x => x.ImplementationType == typeof(BroadcastLocations));
        }

        [Test]
        public void AddObservers_ShouldAddBroadcastLocations_AsAnObserver()
        {
            services.AddObservers();

            services.ShouldContain(x => x.ImplementationType == typeof(BroadcastLocations) && x.ServiceType == typeof(IObserver<List<VehicleLocation>>));
        }

        [Test]
        public void AddObservers_ShouldAddBroadcastLocations_AsAScopeService()
        {
            services.AddObservers();
            
            services.ShouldContain(x => x.ImplementationType == typeof(BroadcastLocations) && x.Lifetime == ServiceLifetime.Scoped);
        }
    }
}
