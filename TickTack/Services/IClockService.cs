using TickTack.Models;

namespace TickTack.Services;

public interface IClockService
{
    List<IClock> AvailableClocks { get; }
    IClock Current { get; set; }
    DateTime GetTime();
}