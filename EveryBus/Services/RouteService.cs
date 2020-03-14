using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using EveryBus.Domain;
using EveryBus.Domain.Models;
using EveryBus.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace EveryBus.Services
{
    public class RouteService : IRouteService
    {
        private readonly HttpClient _httpClient;
        private readonly Uri address;
        private readonly BusContext _busContext;

        public RouteService(IHttpClientFactory _httpClientFactory, IConfiguration _configuration, BusContext _busContext)
        {
            _httpClient = _httpClientFactory.CreateClient("polling");
            address = _configuration.GetValue<Uri>("lothian:address");
            this._busContext = _busContext;
        }

        public async void CreateRoutes()
        {
            var hasStops = _busContext.Stops.Any();
            var hasServices = _busContext.Services.Any();
            var hasRoutes = _busContext.Routes.Any();

            if (!hasStops && !hasServices && !hasRoutes)
            {
                var result = await _httpClient.GetAsync(address + "services");
                var resultString = await result.Content.ReadAsStringAsync();
                var servicesJson = JsonSerializer.Deserialize<ServicesResponse>(resultString);
                await _busContext.AddRangeAsync(servicesJson.services);
                await _busContext.SaveChangesAsync();
            }
        }
    }
}