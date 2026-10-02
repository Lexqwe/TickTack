using Microsoft.Extensions.Logging;
using TickTack.Services;

namespace TickTack;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif  
        builder.Services.AddSingleton<WakeUpService>();
        builder.Services.AddSingleton<IClockService, ClockService>();
        builder.Services.AddTransient<MainPage>();

        return builder.Build();
    }
}
