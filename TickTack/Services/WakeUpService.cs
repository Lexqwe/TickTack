namespace TickTack.Services;

using System.Text.Json;

public class WakeUpService
{
    public SortedSet<TimeOnly> CurrentAlarms { get; private set; } = new();
    private readonly string _path = System.IO.Path.Combine(FileSystem.AppDataDirectory, "alarms.json");
    private string? _lastTriggeredKey;
    public WakeUpService()
    {
        LoadAlarms();
    }

    public void AddAlarm(TimeOnly alarm)
    {
        if (CurrentAlarms.Add(alarm))
        {
            SaveAlarms();
        }
    }

    public void DeleteAlarm(TimeOnly alarm)
    {
        CurrentAlarms.Remove(alarm);
        SaveAlarms();
    }

    public void LoadAlarms()
    {
        if (!File.Exists(_path))
        {
            CurrentAlarms = new SortedSet<TimeOnly>();
            return;
        }
        var json = File.ReadAllText(_path);
        var list = JsonSerializer.Deserialize<SortedSet<TimeOnly>>(json);
        CurrentAlarms = list ?? new SortedSet<TimeOnly>();
    }

    public void SaveAlarms()
    {
        var json = JsonSerializer.Serialize(CurrentAlarms);
        File.WriteAllText(_path, json);
    }

    public bool CheckAlarms(TimeOnly currentTime)
    {
        var key = $"{currentTime.Hour:00}:{currentTime.Minute:00}";
        if (key == _lastTriggeredKey)
        {
            return false;
        }
        bool trigger = CurrentAlarms.Any(a => a.Hour == currentTime.Hour && a.Minute == currentTime.Minute);
        if (trigger)
        {
            _lastTriggeredKey = key;
        }
        return trigger;
    }
}
