using System.Linq;
using EveryBus.Domain;
using EveryBus.Domain.Models;
using EveryBus.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace EveryBus.Services
{
    public class RouteColourService : IRouteColourService
    {
        private readonly IMemoryCache _cache;
        private readonly IServiceScopeFactory _scopeFactory;

        public RouteColourService(IMemoryCache cache, IServiceScopeFactory scopeFactory)
        {
            _cache = cache;
            _scopeFactory = scopeFactory;

            SetupColours();
        }

        private void SetupColours()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var busContext = scope.ServiceProvider.GetRequiredService<BusContext>();
                
                var services = busContext.Services.ToList();

                foreach (var service in services)
                {
                    var routeColours = new RouteColours
                    {
                        Name = service.Name,
                        Colour = service.Color,
                        TextColor = service.TextColor
                    };
                    Set(routeColours);
                }
            }

        }

        public RouteColours Get(string routeName)
        {
            if (routeName != null)
            {
                return _cache.Get<RouteColours>($"route:{routeName}");
            }

            return new RouteColours { Colour = "#000000"};
        }

        public void Set(RouteColours routeColours)
        {
            _cache.Set($"route:{routeColours.Name}", routeColours);
        }
    }
}