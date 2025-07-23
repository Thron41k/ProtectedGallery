using MauiApp2.ViewModels;

namespace MauiApp2.Views;

public partial class GalleryPage : ContentPage
{
	public GalleryPage(GalleryViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
	}
}