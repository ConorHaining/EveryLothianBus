using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using EveryBus.Domain;
using EveryBus.Domain.Models;
using EveryBus.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace EveryBus.Services
{
    public class RouteService : IRouteService
    {
        private readonly HttpClient _httpClient;
        private readonly Uri tfeOpenDataAddress;
        private readonly Uri lothainAddress;
        private readonly BusContext _busContext;

        public RouteService(IHttpClientFactory _httpClientFactory, IConfiguration _configuration, BusContext _busContext)
        {
            _httpClient = _httpClientFactory.CreateClient("polling");
            tfeOpenDataAddress = _configuration.GetValue<Uri>("tfeopendata:address");
            lothainAddress = _configuration.GetValue<Uri>("lothianApi:address");
            this._busContext = _busContext;
        }

        public async void CreateRoutes()
        {
            var hasStops = _busContext.Stops.Any();
            var hasServices = _busContext.Services.Any();
            var hasRoutes = _busContext.Routes.Any();

            if (!hasStops && !hasServices && !hasRoutes)
            {
                var result = await _httpClient.GetAsync(tfeOpenDataAddress + "services");
                var resultString = await result.Content.ReadAsStringAsync();
                var servicesJson = JsonSerializer.Deserialize<ServicesResponse>(resultString);
                servicesJson = await getRouteColoursAsync(servicesJson).ConfigureAwait(false);

                await _busContext.AddRangeAsync(servicesJson.services);
                await _busContext.SaveChangesAsync();
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