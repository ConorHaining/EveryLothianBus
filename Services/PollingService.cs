using System;
using System.Net.Http;
using System.Timers;
using EveryBus.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace EveryBus.Services
{
    public class PollingService : IPollingService
    {
        private readonly HttpClient _httpClient;
        private readonly Uri address;
        private Timer Timer;

        public PollingService(IHttpClientFactory _httpClientFactory, IConfiguration _configuration)
        {
            _httpClient = _httpClientFactory.CreateClient();
            address = _configuration.GetValue<Uri>("lothian.address");

            Timer = new Timer(_configuration.GetValue<long>("lothian.pollInterval", 150000));
            Timer.Elapsed += Poll;
            Timer.AutoReset = true;
            Timer.Enabled = true;
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

        private void Poll(object source, ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);
            var result = _httpClient.GetAsync(address);
        }
    }
}