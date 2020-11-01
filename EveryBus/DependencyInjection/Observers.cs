using EveryBus.Domain.Models;
using EveryBus.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace EveryBus.DependencyInjection
{
    public static class Observers
    {
        public static IServiceCollection AddObservers(this IServiceCollection services)
        {
            services.AddScoped<IObserver<List<VehicleLocation>>, PersistLocations>();
            services.AddScoped<IObserver<List<VehicleLocation>>, BroadcastLocations>();

            return services;
        }
    }
}
