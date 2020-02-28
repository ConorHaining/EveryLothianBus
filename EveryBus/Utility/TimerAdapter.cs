using System.Timers;
using EveryBus.Utility.Interfaces;

public class TimerAdapter : Timer, ITimer
{
    public TimerAdapter()
    {
    }

    public TimerAdapter(double interval) : base(interval)
    {
    }
}