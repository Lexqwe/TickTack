namespace TickTack.Models;

public class SystemClock : IClock
{
    public string Name => "Системные";
    public DateTime Current => DateTime.Now;
}
