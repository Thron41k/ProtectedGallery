using CommunityToolkit.Mvvm.ComponentModel;
using ProtectedGallery.Models;
using CommunityToolkit.Mvvm.Input;
using ProtectedGallery.Helpers;
using ProtectedGallery.Services.Interfaces;


namespace ProtectedGallery.ViewModels;


[QueryProperty(nameof(File), "File")]
public partial class ImageViewViewModel(IFileService fileService) : ObservableObject
{
    [ObservableProperty] 
    private FileItem? _file;

    [ObservableProperty]
    private ImageSource? _imageSource;

    partial void OnFileChanged(FileItem? value)
    {
        if(File != null)
            ImageSource = ImageSource.FromUri(new Uri(File.Uri!));
    }

    [RelayCommand]
    private async Task DeleteFileAsync()
    {
        if(File == null)return;
        var confirmed = await UiHelpers.ShowConfirm("Подтверждение", $"Удалить {File.Name}?");
        if (!confirmed)
            return;

        var success = await fileService.DeleteFileAsync(File.Uri!);
        if (success)
        {
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            await UiHelpers.ShowAlert("Ошибка", $"Не удалось удалить файл {File.Name}"); ;
        }
    }
}
