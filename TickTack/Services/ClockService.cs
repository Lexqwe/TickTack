using TickTack.Models;

namespace TickTack.Services;

public class ClockService : IClockService
{
    public List<IClock> AvailableClocks { get; } = new()
    {
        new SystemClock(),
        new CustomClock(DateTime.Now)
    };
    public IClock Current { get; set; }
    public ClockService()
    {
        Current = AvailableClocks[0];
    }
    public DateTime GetTime() => Current.Current;
}
