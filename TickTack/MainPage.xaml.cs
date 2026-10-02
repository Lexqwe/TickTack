namespace TickTack;

using TickTack.Drawables;
using TickTack.Models;
using TickTack.Services;

public partial class MainPage : ContentPage
{
    private readonly IClockService _clockService;
    private readonly ClockDrawable _drawable;
    private readonly WakeUpService _alarm;
    private readonly PeriodicTimer _periodicTimer;
    private CancellationTokenSource _cts= new();

    public MainPage(IClockService clockService, WakeUpService alarm)
    {
        InitializeComponent();
        _clockService = clockService;
        _alarm = alarm;
        _drawable = new ClockDrawable();
        _periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(1));
        ClockView.Drawable = _drawable;
    }

    protected override void OnAppearing()
    {   
        base.OnAppearing();
        _cts = new CancellationTokenSource();
        _ = RunClockAsync(_cts.Token);
        AlarmList.ItemsSource = _alarm.CurrentAlarms;
    }

    protected override void OnDisappearing()
    {
        _cts.Cancel();
        base.OnDisappearing();
    }

    private async Task RunClockAsync(CancellationToken ct)
    {
        try
        {
            while (await _periodicTimer.WaitForNextTickAsync(ct))
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Refresh();
                    if (_alarm.CheckAlarms(TimeOnly.FromDateTime(_clockService.GetTime())))
                    {
                        _ = DisplayAlertAsync("Будильник", "Пора родной пора!", "OK");
                    }
                });
            }
        }
        catch (OperationCanceledException) { }
    }

    private void Refresh()
    {
        _drawable.CurrentTime = TimeOnly.FromDateTime(_clockService.GetTime());
        ClockView.Invalidate();
    }

    private void OnApplyClicked(object sender, EventArgs e)
    {
        var timeInput = TimeInput.Time ?? DateTime.Now.TimeOfDay;
        double speed;
        var start = new DateTime(
            DateTime.Today.Year,
            DateTime.Today.Month,
            DateTime.Today.Day,
            timeInput.Hours,
            timeInput.Minutes,
            0);
        if (!double.TryParse(SpeedInput.Text, out speed))
        {
            speed = 1;
        }
        _clockService.Current = new CustomClock(start, speed);
        Refresh();
    }

    private void OnToSystemClicked(object sender, EventArgs e)
    {
        _clockService.Current = new SystemClock();
    }

    private void OnAddAlarmClicked(object sender, EventArgs e)
    {
        var alarmInput = AlarmInput.Time;
        if (alarmInput == null)
        {
            return;
        }
        _alarm.AddAlarm(TimeOnly.FromTimeSpan(alarmInput.Value));
        AlarmList.ItemsSource = null;
        AlarmList.ItemsSource = _alarm.CurrentAlarms;
    }

    private void OnDeleteAlarmClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var alarm = (TimeOnly)button.CommandParameter;
        _alarm.DeleteAlarm(alarm);
        AlarmList.ItemsSource = null;
        AlarmList.ItemsSource = _alarm.CurrentAlarms;
    }
}
