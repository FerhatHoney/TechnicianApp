using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using TechnicianApp.Core.Interfaces;
using TechnicianApp.Infrastructure.Repositories;
using TechnicianApp.Infrastructure.Services;
using Plugin.LocalNotification;

namespace TechnicianApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseLocalNotification()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            // MudBlazor UI Servislerini Kaydet
            builder.Services.AddMudServices();

            // Kendi Servislerimizi Kaydet (Dependency Injection)
            // AuthService tek bir örnek (Singleton) olarak yaşar
            builder.Services.AddSingleton<IAuthService, AuthService>();

            // BaseRepository generic olduğu için typeof ile Transient (her çağrıda yeni) olarak ekliyoruz
            builder.Services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            return builder.Build();
        }
    }
}