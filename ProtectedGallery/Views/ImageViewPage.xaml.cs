using ProtectedGallery.ViewModels;

namespace ProtectedGallery.Views;

public partial class ImageViewPage : ContentPage
{
	public ImageViewPage(ImageViewViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
	}
}