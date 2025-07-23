using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp2.Models;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp2.Helpers;
using MauiApp2.Services.Interfaces;


namespace MauiApp2.ViewModels;


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
