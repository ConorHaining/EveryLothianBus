using System;

public class PollingService : IPollingService
{
    private readonly Uri address;
    private readonly TimeSpan pollInterval;

    public PollingService(Uri address, TimeSpan pollInterval)
    {
        this.address = address;
        this.pollInterval = pollInterval;
    }

    public PollingStatus Start()
    {
        throw new NotImplementedException();
    }

    public PollingStatus Status()
    {
        throw new NotImplementedException();
    }

    public PollingStatus Stop()
    {
        throw new NotImplementedException();
    }
}