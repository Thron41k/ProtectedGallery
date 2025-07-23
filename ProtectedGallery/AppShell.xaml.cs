using ProtectedGallery.Views;

namespace ProtectedGallery;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(ImageViewPage), typeof(ImageViewPage));
    }
}