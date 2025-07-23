using ProtectedGallery.ViewModels;

namespace ProtectedGallery.Views;

public partial class GalleryPage : ContentPage
{
	public GalleryPage(GalleryViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
	}
}