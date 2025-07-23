using Android.App;
using Android.Content;
using Android.Content.PM;

namespace ProtectedGallery;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
    {
        base.OnActivityResult(requestCode, resultCode, data);

        var serviceProvider = IPlatformApplication.Current?.Services;
        if (serviceProvider == null)
            return;

        var receiver = serviceProvider.GetService<ProtectedGallery.Services.Interfaces.IActivityResultReceiver>();
        receiver?.OnActivityResult(requestCode, resultCode, data);
    }
}