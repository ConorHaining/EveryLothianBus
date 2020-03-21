using System.Timers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Collections.Generic;
using System;
using EveryBus.Domain.Models;

namespace EveryBus.Services
{
    public class DataRefreshService : IHostedService
    {
        private readonly BusLocationsProvider _busLocationsProvider;
        private readonly IConfiguration _configuration;
        private IEnumerable<IObserver<VehicleLocation[]>> _observers;
        private System.Timers.Timer _timer;
        
        public DataRefreshService(BusLocationsProvider busLocationsProvider, IConfiguration configuration, IEnumerable<IObserver<VehicleLocation[]>> observers)
        {
            _busLocationsProvider = busLocationsProvider;
            _configuration = configuration;
            _observers = observers;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new System.Timers.Timer(_configuration.GetValue<long>("tfeopendata:pollInterval", 15000));
            _timer.Elapsed += PollAsync;
            _timer.AutoReset = true;
            _timer.Enabled = true;

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Enabled = false;
            return Task.CompletedTask;
        }

        private async void PollAsync(object source, ElapsedEventArgs e)
        {
            var data = await _busLocationsProvider.UpdateData(default(CancellationToken)).ConfigureAwait(false);

            foreach (var observer in _observers)
            {
                observer.OnNext(data.vehicleLocations);
            }
        }
    }
}