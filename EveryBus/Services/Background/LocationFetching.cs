using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using EveryBus.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EveryBus.Services.Background
{
    public class LocationFetching : BackgroundService
    {
        private readonly ILogger<LocationFetching> _logger;
        private readonly IConfiguration _configuration;
        private IEnumerable<IObserver<List<VehicleLocation>>> _observers;
        private readonly HttpClient _httpClient;
        private readonly Uri _pollAddress;

        public LocationFetching(
            ILogger<LocationFetching> logger,
            IConfiguration configuration,
            IEnumerable<IObserver<List<VehicleLocation>>> observers,
            IHttpClientFactory _httpClientFactory
            )
        {
            _logger = logger;
            _configuration = configuration;
            _observers = observers;
            _httpClient = _httpClientFactory.CreateClient("polling");
            _pollAddress = _configuration.GetValue<Uri>("tfeopendata:address");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var pollInterval = _configuration.GetValue<long>("tfeopendata:pollInterval", 15000);
            _logger.LogInformation("LocationFetching is starting.");

            stoppingToken.Register(() => _logger.LogDebug("LocationFetching is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                var vehicleUpdatesResponse = await PollAsync();

                foreach (var observer in _observers)
                {
                    observer.OnNext(vehicleUpdatesResponse?.vehicleLocations);
                }

                await Task.Delay(TimeSpan.FromMilliseconds(pollInterval));
            }

        }
        private async Task<VehicleLocationResponse> PollAsync()
        {
            _logger.LogInformation("Request raised at {0}.", DateTime.Now);

            HttpResponseMessage result;
            try
            {
                result = await _httpClient.GetAsync(_pollAddress + "vehicle_locations");
                result.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                _logger.LogWarning("Request has been unsucessful.");
                return default;
            }

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var vehicleUpdates = await result.Content.ReadAsStringAsync();
            var vehicleUpdatesResponse = JsonSerializer.Deserialize<VehicleLocationResponse>(vehicleUpdates, jsonOptions);
            _logger.LogInformation("Request completed at at {0}.", DateTime.Now);

            return vehicleUpdatesResponse;
        }

    }
}