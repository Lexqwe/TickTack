namespace TickTack.Models;

public interface IClock
{
    string Name { get; }
    DateTime Current { get; }
}
