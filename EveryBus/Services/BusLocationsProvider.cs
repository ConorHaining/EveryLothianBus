using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using EveryBus.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace EveryBus.Services
{
    public class BusLocationsProvider
    {
        private readonly HttpClient _httpClient;
        private readonly Uri _pollAddress;
        private List<IObserver<VehicleLocation[]>> _observers;

        public BusLocationsProvider(IHttpClientFactory _httpClientFactory, IConfiguration _configuration)
        {
            _httpClient = _httpClientFactory.CreateClient("polling");
            _pollAddress = _configuration.GetValue<Uri>("tfeopendata:address");
        }

        public async Task<VehicleLocationResponse> UpdateData(CancellationToken cancellationToken)
        {
            Console.WriteLine("The Elapsed event was raised at {0}", DateTime.Now);

            HttpResponseMessage result;
            try
            {
                result = await _httpClient.GetAsync(_pollAddress + "vehicle_locations");
                result.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                return default(VehicleLocationResponse);
            }

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var vehicleUpdates = await result.Content.ReadAsStringAsync();
            var vehicleUpdatesResponse = JsonSerializer.Deserialize<VehicleLocationResponse>(vehicleUpdates, jsonOptions);

            return vehicleUpdatesResponse;
        }
    }
}