using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Timers;
using EveryBus.Domain.Models;
using EveryBus.Services.Interfaces;
using EveryBus.Utility.Interfaces;
using Microsoft.Extensions.Configuration;

namespace EveryBus.Services
{
    public class PollingService : IPollingService
    {
        private readonly HttpClient _httpClient;
        private readonly Uri tfeOpenDataaddress;
        private ITimer _timer;
        private List<IObserver<VehicleLocation[]>> _observers;

        public PollingService(IHttpClientFactory _httpClientFactory, IConfiguration _configuration)
        {
            _httpClient = _httpClientFactory.CreateClient("polling");
            tfeOpenDataaddress = _configuration.GetValue<Uri>("tfeopendata:address");

            _timer = new TimerAdapter(_configuration.GetValue<long>("lothian:pollInterval", 150000));
            _timer.Elapsed += PollAsync;
            _timer.AutoReset = true;
            _timer.Enabled = true;

            _observers = new List<IObserver<VehicleLocation[]>>();
        }

        public PollingStatus Start()
        {
            _timer.Start();
            _timer.Enabled = true;
            return PollingStatus.Started;
        }

        public PollingStatus Status()
        {
            if (_timer.Enabled)
            {
                return PollingStatus.Running;
            }
            else
            {
                return PollingStatus.Stopped;
            }
        }

        public PollingStatus Stop()
        {
            _timer.Stop();
            _timer.Enabled = false;
            return PollingStatus.Stopped;
        }

        public IDisposable Subscribe(IObserver<VehicleLocation[]> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }

            return new Unsubscriber(_observers, observer);
        }

        private async void PollAsync(object source, ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);
            
            HttpResponseMessage result;
            try
            {
                result = await _httpClient.GetAsync(tfeOpenDataaddress + "vehicle_locations");
                result.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                return;
            }

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var vehicleUpdates = await result.Content.ReadAsStringAsync();
            var vehicleUpdatesJson = JsonSerializer.Deserialize<VehicleLocationResponse>(vehicleUpdates, jsonOptions);

            foreach (var observer in _observers)
            {
                observer.OnNext(vehicleUpdatesJson.vehicleLocations);
            }
        }
    }

    internal class Unsubscriber : IDisposable
    {
        private List<IObserver<VehicleLocation[]>> _observers;
        private IObserver<VehicleLocation[]> _observer;

        public Unsubscriber(List<IObserver<VehicleLocation[]>> observers, IObserver<VehicleLocation[]> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }

        public void Dispose()
        {
            if (!(_observer == null)) _observers.Remove(_observer);
        }
    }
}