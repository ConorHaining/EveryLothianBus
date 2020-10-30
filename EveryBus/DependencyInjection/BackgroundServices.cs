using EveryBus.Services.Background;
using Microsoft.Extensions.DependencyInjection;

namespace EveryBus.DependencyInjection
{
    public static class BackgroundServices
    {
        public static IServiceCollection AddBackgroundServices(this IServiceCollection services)
        {
            services.AddHostedService<LocationFetching>();
            services.AddHostedService<RouteFetching>();

            return services;
        }
    }
}
