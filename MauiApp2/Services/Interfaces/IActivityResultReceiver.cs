namespace MauiApp2.Services.Interfaces
{
    public interface IActivityResultReceiver
    {
#if ANDROID
        void OnActivityResult(int requestCode, Android.App.Result resultCode, Android.Content.Intent? data);
#endif
    }
}
