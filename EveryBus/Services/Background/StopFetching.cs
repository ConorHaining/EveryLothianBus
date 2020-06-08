using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using EveryBus.Domain;
using EveryBus.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EveryBus.Services.Background
{
    public class StopFetching : BackgroundService
    {
        private readonly ILogger<StopFetching> _logger;
        private readonly IConfiguration _configuration;
        private IEnumerable<IObserver<VehicleLocation[]>> _observers;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly HttpClient _httpClient;
        private readonly Uri _pollAddress;
        private readonly Uri _lothainAddress;
        private readonly long _pollInterval;

        public StopFetching(
            ILogger<StopFetching> logger,
            IConfiguration configuration,
            IEnumerable<IObserver<VehicleLocation[]>> observers,
            IHttpClientFactory _httpClientFactory,
            IServiceScopeFactory scopeFactory
            )
        {
            _logger = logger;
            _configuration = configuration;
            _observers = observers;
            _httpClient = _httpClientFactory.CreateClient("polling");
            _pollAddress = _configuration.GetValue<Uri>("tfeopendata:address");
            _lothainAddress = _configuration.GetValue<Uri>("lothianApi:address");
            _pollInterval = _configuration.GetValue<long>("tfeopendata:pollInterval");
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("StopFetching is starting.");

            stoppingToken.Register(() => _logger.LogDebug("StopFetching is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMinutes(1));

                var stops = await PollAsync();

                var existingStops = GetExistingStops();

                UpdateStops(existingStops, stops);

                await Task.Delay(TimeSpan.FromDays(1));
            }

        }

        private void UpdateStops(List<Stop> existingStops, List<Stop> newStops)
        {
            foreach (var stop in existingStops)
            {
                var findStop = newStops.Where(x => x.StopId == stop.StopId).FirstOrDefault();

                if (findStop != null)
                {
                    stop.Name = findStop.Name;
                    stop.Identifier = findStop.Identifier;
                    stop.Locality = findStop.Locality;
                    stop.Orientation = findStop.Orientation;
                    stop.Direction = findStop.Direction;
                    stop.Latitude = findStop.Latitude;
                    stop.Longitude = findStop.Longitude;
                    stop.ServiceType = findStop.ServiceType;

                    newStops.Remove(findStop);
                }
            }

            using (var scope = _scopeFactory.CreateScope())
            {
                var busContext = scope.ServiceProvider.GetRequiredService<BusContext>();

                busContext.UpdateRange(existingStops);
                busContext.AddRange(newStops);
                busContext.SaveChanges();
            }
        }

        private List<Stop> GetExistingStops()
        {
            List<Stop> stopsToUpdate = new List<Stop>();

            using (var scope = _scopeFactory.CreateScope())
            {
                var busContext = scope.ServiceProvider.GetRequiredService<BusContext>();

                stopsToUpdate = busContext.Stops.Where(x => x.StopId != null).ToList();
            }

            return stopsToUpdate;
        }

        public async Task<List<Stop>> PollAsync()
        {
            _logger.LogInformation("Request raised at {0}.", DateTime.Now);

            var result = await _httpClient.GetAsync(_pollAddress + "stops");
            if (!result.IsSuccessStatusCode)
            {
                _logger.LogWarning("Request has been unsucessful with code {0}.", result.StatusCode);
                return default(List<Stop>);
            }

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var stops = await result.Content.ReadAsStringAsync();
            var stopsResponse = JsonSerializer.Deserialize<StopResponse>(stops, jsonOptions);
            _logger.LogInformation("Request completed at at {0}.", DateTime.Now);

            return stopsResponse.Stops;
        }
    }
}