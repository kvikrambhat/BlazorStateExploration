using Microsoft.AspNetCore.Components.WebView.Maui;
using BlazorHybridStateInjected.Data;
using HybridTodo.State;

namespace BlazorHybridStateInjected;

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
            });

        builder.Services.AddMauiBlazorWebView();
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
#endif

        builder.Services.AddSingleton<WeatherForecastService>();
        //GRL : Step 4 : Make the state object available to be injected into the razor library component.
        builder.Services.AddScoped<TodoState>();

        return builder.Build();
    }
}
