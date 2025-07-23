using CommunityToolkit.Maui;
using MauiApp2.Services;
using MauiApp2.Services.Interfaces;
using MauiApp2.ViewModels;
using MauiApp2.Views;
using Microsoft.Extensions.Logging;


namespace MauiApp2;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        builder.Services.AddSingleton<IPinService, PinService>();
#if ANDROID
        builder.Services.AddSingleton<IFileService, FileServiceAndroid>();
        builder.Services.AddSingleton(sp =>
            (IActivityResultReceiver)sp.GetRequiredService<IFileService>());
#endif
        builder.Services.AddTransient<PinViewModel>();
        builder.Services.AddTransient<GalleryViewModel>();
        builder.Services.AddTransient<ImageViewViewModel>();

        builder.Services.AddTransient<PinPage>();
        builder.Services.AddTransient<GalleryPage>();
        builder.Services.AddTransient<ImageViewPage>();
        return builder.Build();
    }
}