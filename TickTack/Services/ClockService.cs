using TickTack.Models;

namespace TickTack.Services;

public class ClockService
{
    private Clock _clock = new();
    private IDispatcherTimer _timer;
    public event Action? TimeUpdated;
    public ClockService(IDispatcher dispatcher)
    {
        _timer = dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.IsRepeating = true;
        _timer.Tick += OnTick;
        SetTime();
    }

    public Clock GetClock()
    {
        return _clock;
    }

    public void SetTime() {
        _clock.CurrentTime = TimeOnly.FromDateTime(DateTime.Now);
    }

    private void OnTick(object? sender, EventArgs e)
    {
        SetTime();
        TimeUpdated?.Invoke();
    }

    public void Start()
    {
        if (_timer.IsRunning) return;
        _timer.Start();
    }

    public void Stop()
    {
        _timer.Stop();
    }

    //public void ChangeSpeed(decimal speed) { }
}
