using Application.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.Logging;
using UI.ViewModels;
using UI.Views;

namespace UI;

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

        builder.Services.AddSingleton<IAuthService, FirebaseAuthService>();
        builder.Services.AddSingleton<ICurrencyService, CurrencyApiService>();
        builder.Services.AddSingleton<INewsService, RssNewsService>();
        builder.Services.AddSingleton<ITodoService, FirebaseDbService>();
        builder.Services.AddSingleton<IWeatherService, MgmWeatherService>();
        
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<LoginPage>();
        
        builder.Services.AddTransient<RegisterViewModel>();
        builder.Services.AddTransient<RegisterPage>();
        
        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<HomePage>();
        
        builder.Services.AddTransient<SettingsViewModel>();
        builder.Services.AddTransient<SettingsPage>();
        
        builder.Services.AddTransient<CurrencyViewModel>();
        builder.Services.AddTransient<CurrencyPage>();
        
        builder.Services.AddTransient<WeatherViewModel>();
        builder.Services.AddTransient<WeatherPage>();
        
        builder.Services.AddTransient<NewsViewModel>();
        builder.Services.AddTransient<NewsPage>();
        
        builder.Services.AddTransient<NewsDetailViewModel>();
        builder.Services.AddTransient<NewsPage>();
        
        builder.Services.AddTransient<TodoViewModel>();
        builder.Services.AddTransient<TodoPage>();

        builder.Services.AddTransient<TodoDetailViewModel>();
        builder.Services.AddTransient<TodoDetailPage>();
        
        return builder.Build();
    }
}