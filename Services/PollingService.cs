using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Timers;
using EveryBus.Domain.Models;
using EveryBus.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace EveryBus.Services
{
    public class PollingService : IPollingService, IObservable<VehicleLocation>
    {
        private readonly HttpClient _httpClient;
        private readonly Uri address;
        private Timer Timer;
        private List<IObserver<VehicleLocation>> observers;

        public PollingService(IHttpClientFactory _httpClientFactory, IConfiguration _configuration)
        {
            _httpClient = _httpClientFactory.CreateClient("polling");
            address = _configuration.GetValue<Uri>("lothian:address");

            Timer = new Timer(_configuration.GetValue<long>("lothian:pollInterval", 150000));
            Timer.Elapsed += PollAsync;
            Timer.AutoReset = true;
            Timer.Enabled = true;

            observers = new List<IObserver<VehicleLocation>>();
        }

        public PollingStatus Start()
        {
            Timer.Start();
            Timer.Enabled = true;
            return PollingStatus.Started;
        }

        public PollingStatus Status()
        {
            if (Timer.Enabled)
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
            Timer.Stop();
            Timer.Enabled = false;
            return PollingStatus.Stopped;
        }

        public IDisposable Subscribe(IObserver<VehicleLocation> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }

            return new Unsubscriber(observers, observer);
        }

        private async void PollAsync(object source, ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);
            var result = await _httpClient.GetAsync(address);
            result.EnsureSuccessStatusCode();
        }
    }

    internal class Unsubscriber : IDisposable
    {
        private List<IObserver<VehicleLocation>> _observers;
        private IObserver<VehicleLocation> _observer;

        public Unsubscriber(List<IObserver<VehicleLocation>> observers, IObserver<VehicleLocation> observer)
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