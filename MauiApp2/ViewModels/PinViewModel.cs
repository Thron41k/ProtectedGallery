using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp2.Helpers;
using MauiApp2.Services.Interfaces;

namespace MauiApp2.ViewModels;

public partial class PinViewModel : ObservableObject
{
    private readonly IPinService? _pinService;

    public string? PinInput { get; set; }
    public string? ConfirmPin { get; set; }
    public bool IsFirstLaunch { get; private set; }
    public ICommand? SubmitCommand { get; }

    public PinViewModel(IPinService pinService)
    {
        _pinService = pinService;
        SubmitCommand = new Command(async void () => await SubmitAsync());
        _ = InitAsync();
    }

    public PinViewModel()
    {
        
    }

    private async Task InitAsync()
    {
        if (_pinService == null) return;
        IsFirstLaunch = !await _pinService.IsPinSetAsync();
        OnPropertyChanged(nameof(IsFirstLaunch));
    }

    private async Task SubmitAsync()
    {
        if (_pinService == null) return;
        if (string.IsNullOrWhiteSpace(PinInput) || PinInput.Length != 4)
        {
            await UiHelpers.ShowAlert("Ошибка", "Введите 4-значный PIN-код");
            return;
        }

        if (IsFirstLaunch)
        {
            if (PinInput != ConfirmPin)
            {
                await UiHelpers.ShowAlert("Ошибка", "PIN-коды не совпадаю");
                return;
            }

            await _pinService.SetPinAsync(PinInput);
            await Shell.Current.GoToAsync("//GalleryPage");
        }
        else
        {
            if (await _pinService.ValidatePinAsync(PinInput))
            {
                await Shell.Current.GoToAsync("//GalleryPage");
            }
            else
            {
                await UiHelpers.ShowAlert("Ошибка", "Неверный PIN-код");
            }
        }
    }
}