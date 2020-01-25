public interface IPollingService
{
    PollingStatus Start();
    PollingStatus Stop();
    PollingStatus Status();
}
