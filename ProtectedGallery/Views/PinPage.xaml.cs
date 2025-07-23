using ProtectedGallery.ViewModels;

namespace ProtectedGallery.Views;

public partial class PinPage : ContentPage
{
	public PinPage(PinViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}