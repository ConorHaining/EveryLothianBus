using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using EveryBus.Domain;
using EveryBus.Domain.Models;
using EveryBus.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EveryBus.Services
{
    public class RouteService : IRouteService
    {
        private readonly HttpClient _httpClient;
        private readonly Uri tfeOpenDataAddress;
        private readonly Uri lothainAddress;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IRouteColourService _routeColourService;

        public RouteService(IHttpClientFactory _httpClientFactory,
                            IConfiguration _configuration,
                            IServiceScopeFactory scopeFactory,
                            IRouteColourService routeColourService)
        {
            _httpClient = _httpClientFactory.CreateClient("polling");
            tfeOpenDataAddress = _configuration.GetValue<Uri>("tfeopendata:address");
            lothainAddress = _configuration.GetValue<Uri>("lothianApi:address");
            _scopeFactory = scopeFactory;
            _routeColourService = routeColourService;
        }

        public async void CreateRoutes()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var busContext = scope.ServiceProvider.GetRequiredService<BusContext>();

                var hasStops = busContext.Stops.Any();
                var hasServices = busContext.Services.Any();
                var hasRoutes = busContext.Routes.Any();

                if (!hasStops && !hasServices && !hasRoutes)
                {
                    var result = await _httpClient.GetAsync(tfeOpenDataAddress + "services");
                    var resultString = await result.Content.ReadAsStringAsync();
                    var servicesJson = JsonSerializer.Deserialize<ServicesResponse>(resultString);
                    servicesJson = await getRouteColoursAsync(servicesJson).ConfigureAwait(false);

                    await busContext.AddRangeAsync(servicesJson.services);
                    await busContext.SaveChangesAsync();
                }
            }
        }

        public IEnumerable<Service> GetRoutes()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var busContext = scope.ServiceProvider.GetRequiredService<BusContext>();

                return busContext.Services.ToList();
            }
        }

        private async Task<ServicesResponse> getRouteColoursAsync(ServicesResponse servicesResponse)
        {
            foreach (var service in servicesResponse.services)
            {
                var routeId = service.Name;

                var result = await _httpClient.GetAsync($"{lothainAddress}route.php?service_name={routeId}");
                if (result.IsSuccessStatusCode)
                {
                    var resultString = await result.Content.ReadAsStringAsync();
                    using (var jsonDoc = JsonDocument.Parse(resultString))
                    {
                        var root = jsonDoc.RootElement;
                        service.Color = root.GetProperty("color").GetString();
                        service.TextColor = root.GetProperty("text_color").GetString();
                    };
                }
            }

            return servicesResponse;
        }
    }
}