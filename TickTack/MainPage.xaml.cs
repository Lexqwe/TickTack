namespace TickTack;

using TickTack.Drawables;
using TickTack.Services;

public partial class MainPage : ContentPage
{
    private ClockService _clockService;
    private ClockDrawable _drawable;

    public MainPage(ClockService clockService)
    {
        InitializeComponent();
        _clockService = clockService;
        _drawable = new ClockDrawable();
        ClockView.Drawable = _drawable;
    }

    override protected void OnAppearing()
    {   
        base.OnAppearing();
        _clockService.TimeUpdated += HandleTick;
        _clockService.Start();
    }

    protected override void OnDisappearing()
    {
        _clockService.TimeUpdated -= HandleTick;
        _clockService.Stop();
        base.OnDisappearing();
    }

    private void HandleTick()
    {
        _drawable.CurrentTime = _clockService.GetClock().CurrentTime;
        ClockView.Invalidate();
    }

}
