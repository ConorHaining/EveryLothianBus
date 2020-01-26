namespace EveryBus.Services.Interfaces
{
    public interface IPollingService
    {
        PollingStatus Start();
        PollingStatus Stop();
        PollingStatus Status();
    }
}