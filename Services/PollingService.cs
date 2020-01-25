using System;
using System.Net.Http;
using System.Timers;

public class PollingService : IPollingService
{
    private readonly HttpClient _httpClient;
    private readonly Uri address;
    private Timer Timer;

    public PollingService(HttpClient _httpClient, Uri address, TimeSpan pollInterval)
    {
        this._httpClient = _httpClient;
        this.address = address;

        Timer = new Timer(pollInterval.TotalMilliseconds);
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

    private void Poll(Object source, ElapsedEventArgs e)
    {
        Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);
        var result = _httpClient.GetAsync(address);
    }
}