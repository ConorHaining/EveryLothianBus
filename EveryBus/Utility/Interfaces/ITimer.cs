using System.Timers;

namespace EveryBus.Utility.Interfaces
{
    public interface ITimer
    {
        event ElapsedEventHandler Elapsed;
        double Interval { get; set; }
        bool Enabled { get; set; }
        bool AutoReset { get; set; }
        void Dispose();
        void Start();
        void Stop();
    }
}