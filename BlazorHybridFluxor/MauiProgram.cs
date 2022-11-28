using Microsoft.AspNetCore.Components.WebView.Maui;
using BlazorHybridFluxor.Data;
using Fluxor; //GRL : Step 1 - Add dependencies - Fluxor, Fluxor.Blazor.web, Fluxor.Blazor.Web.ReduxDevTools
using HybridTodo;

namespace BlazorHybridFluxor;

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

        //GRL : Step 2 - Add Fluxor Service to the main app
        builder.Services.AddFluxor(o =>
        {
            o.ScanAssemblies(typeof(TodoControl).Assembly);////GRL : Step 2 Make sure to add all the assemblies that have state defined
            o.ScanAssemblies(typeof(MauiProgram).Assembly);
            o.UseReduxDevTools(rdt =>
            {
                rdt.Name = "My application";
            });
        });

        return builder.Build();
	}
}
