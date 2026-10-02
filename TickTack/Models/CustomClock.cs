namespace TickTack.Models;

public class CustomClock : IClock
{
    public string Name => "Кастомные";

    private readonly DateTime _start;
    private readonly DateTime _base;
    private readonly double _speed;

    public CustomClock(DateTime start, double speed = 1)
    {
        _start = start;
        _base = DateTime.Now;
        _speed = speed;
    }
    public DateTime Current => _start + TimeSpan.FromTicks((long)((DateTime.Now - _base).Ticks * _speed));
}