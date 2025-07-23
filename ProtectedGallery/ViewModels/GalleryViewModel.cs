using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProtectedGallery.Helpers;
using ProtectedGallery.Models;
using ProtectedGallery.Services.Interfaces;

namespace ProtectedGallery.ViewModels;

public partial class GalleryViewModel(IFileService fileService) : ObservableObject
{
    [ObservableProperty]
    private IFileService _fileService = fileService;

    [ObservableProperty]
    private FileItem? _selectedImage;

    [RelayCommand]
    public async Task PickFolder()
    {
        await FileService.PickFolderAsync();
    }

    [RelayCommand]
    public async Task DeleteFile()
    {
        if (SelectedImage == null)
            return;
        var confirmed = await UiHelpers.ShowConfirm("Подтверждение", $"Удалить {SelectedImage.Name}?");
        if (!confirmed)
            return;
        var success = await FileService.DeleteFileAsync(SelectedImage.Uri!);
        if (!success)
            await UiHelpers.ShowAlert("Ошибка", $"Не удалось удалить файл {SelectedImage.Name}");
    }

    [RelayCommand]
    public async Task OpenFile()
    {
        if (SelectedImage == null)
            return;
        await Shell.Current.GoToAsync("/ImageViewPage", new Dictionary<string, object>
        {
            ["File"] = SelectedImage
        });
    }
}
