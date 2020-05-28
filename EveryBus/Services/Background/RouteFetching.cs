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
    public class RouteFetching : BackgroundService
    {
        private readonly ILogger<RouteFetching> _logger;
        private readonly IConfiguration _configuration;
        private IEnumerable<IObserver<VehicleLocation[]>> _observers;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly HttpClient _httpClient;
        private readonly Uri _pollAddress;
        private readonly Uri _lothainAddress;
        private readonly long _pollInterval;

        public RouteFetching(
            ILogger<RouteFetching> logger,
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
            _logger.LogInformation("RouteFetching is starting.");

            stoppingToken.Register(() => _logger.LogDebug("RouteFetching is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                var services = await PollAsync();
                services = await getRouteColoursAsync(services).ConfigureAwait(false);

                var newServices = FindUniqueServices(services.services);

                UpdateServices(newServices);

                await Task.Delay(TimeSpan.FromDays(1));
            }

        }

        private void UpdateServices(List<Service> newServices)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var busContext = scope.ServiceProvider.GetRequiredService<BusContext>();

                busContext.UpdateRange(newServices);
                busContext.SaveChanges();
            }
        }

        private List<Service> FindUniqueServices(List<Service> services)
        {
            List<Service> servicesToUpdate = new List<Service>();

            using (var scope = _scopeFactory.CreateScope())
            {
                var busContext = scope.ServiceProvider.GetRequiredService<BusContext>();

                foreach (var service in services)
                {
                    var existingService = busContext.Services.Include(x => x.Routes)
                                                                .ThenInclude(x => x.Stops)
                                                             .Where(x => x.Name == service.Name)
                                                             .FirstOrDefault();
                    if (existingService == null || existingService == default(Service))
                    {
                        servicesToUpdate.Add(service);
                        continue;
                    }

                    foreach (var route in service.Routes)
                    {
                        var existingRoute = existingService.Routes.Where(x => x.Destination == route.Destination).FirstOrDefault();
                        if (existingRoute == default(Route))
                        {
                            servicesToUpdate.Add(service);
                            continue;
                        }

                        var difference = existingRoute.Stops.Except(route.Stops);

                        if (difference.Count() > 0)
                        {
                            servicesToUpdate.Add(service);
                            continue;
                        }
                    }
                }
            }

            return servicesToUpdate;
        }

        public async Task<ServicesResponse> PollAsync()
        {
            _logger.LogInformation("Request raised at {0}.", DateTime.Now);

            var result = await _httpClient.GetAsync(_pollAddress + "services");
            if (!result.IsSuccessStatusCode)
            {
                _logger.LogWarning("Request has been unsucessful with code {0}.", result.StatusCode);
                return default(ServicesResponse);
            }

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var vehicleUpdates = await result.Content.ReadAsStringAsync();
            var vehicleUpdatesResponse = JsonSerializer.Deserialize<ServicesResponse>(vehicleUpdates, jsonOptions);
            _logger.LogInformation("Request completed at at {0}.", DateTime.Now);

            return vehicleUpdatesResponse;
        }

        private async Task<ServicesResponse> getRouteColoursAsync(ServicesResponse servicesResponse)
        {
            foreach (var service in servicesResponse.services)
            {
                var routeId = service.Name;

                var result = await _httpClient.GetAsync($"{_lothainAddress}route.php?service_name={routeId}");
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