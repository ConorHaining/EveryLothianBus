using EveryBus.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EveryBus.Services.Background
{
    public class VehicleTracking : IHostedService
    {
        private readonly ILogger<VehicleTracking> _logger;
        private readonly IConfiguration _configuration;

        private readonly HttpClient _httpClient;
        private readonly Uri _pollAddress;

        public IServiceProvider Services { get; }

        private Task _executingTask;

        public VehicleTracking(
            ILogger<VehicleTracking> logger,
            IConfiguration configuration,
            IHttpClientFactory _httpClientFactory,
            IServiceProvider services
            )
        {
            _logger = logger;
            _configuration = configuration;
            Services = services;
            _httpClient = _httpClientFactory.CreateClient("polling");
            _pollAddress = _configuration.GetValue<Uri>("tfeopendata:address");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(VehicleTracking)} is starting.");
            _executingTask = ExecuteAsync(cancellationToken);

            if (_executingTask.IsCompleted)
            {
                _logger.LogInformation($"{nameof(VehicleTracking)} was cancelled.");
                return _executingTask;
            }

            return Task.CompletedTask;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var pollInterval = _configuration.GetValue<long>("tfeopendata:pollInterval", 15000);
            
            using var scope = Services.CreateScope();
            var observers = scope.ServiceProvider.GetRequiredService<IEnumerable<IObserver<List<VehicleLocation>>>>();
            while (!cancellationToken.IsCancellationRequested)
            {
                var vehicleUpdatesResponse = await PollAsync();

                foreach (var observer in observers)
                {
                    observer.OnNext(vehicleUpdatesResponse?.vehicleLocations);
                }

                await Task.Delay(TimeSpan.FromMilliseconds(pollInterval));
            }

        }

        private async Task<VehicleLocationResponse> PollAsync()
        {
            _logger.LogDebug("Request raised.");

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


        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_executingTask == null)
            {
                return;
            }

            await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
        }
    }
}