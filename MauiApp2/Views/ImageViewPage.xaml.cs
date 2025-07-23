using MauiApp2.ViewModels;

namespace MauiApp2.Views;

public partial class ImageViewPage : ContentPage
{
	public ImageViewPage(ImageViewViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
	}
}