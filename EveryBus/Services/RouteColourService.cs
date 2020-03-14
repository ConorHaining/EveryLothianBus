using System.Linq;
using EveryBus.Domain;
using EveryBus.Domain.Models;
using EveryBus.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace EveryBus.Services
{
    public class RouteColourService : IRouteColourService
    {
        private readonly IMemoryCache _cache;
        private readonly BusContext _busContext;

        public RouteColourService(IMemoryCache cache, BusContext busContext)
        {
            _cache = cache;
            _busContext = busContext;

            SetupColours();
        }

        private void SetupColours()
        {
            var services = _busContext.Services.ToList();

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