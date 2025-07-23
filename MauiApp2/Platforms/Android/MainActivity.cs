using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Microsoft.Maui.Controls;

namespace MauiApp2
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            var serviceProvider = (this.Application as MauiApplication)?.Services;
            if (serviceProvider == null)
                return;

            var receiver = serviceProvider.GetService<MauiApp2.Services.IActivityResultReceiver>();
            receiver?.OnActivityResult(requestCode, resultCode, data);
        }
    }
}
