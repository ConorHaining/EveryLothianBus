using EveryBus.Domain.Models;

namespace EveryBus.Services.Interfaces
{
    public interface IRouteColourService
    {
        RouteColours Get(string routeName);
        void Set(RouteColours routeColours);
    }
}