using MauiApp2.ViewModels;

namespace MauiApp2.Views;

public partial class PinPage : ContentPage
{
	public PinPage(PinViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}