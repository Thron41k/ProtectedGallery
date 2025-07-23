using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp2.Models;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp2.Services.Interfaces;


namespace MauiApp2.ViewModels;


[QueryProperty(nameof(File), "File")]
[QueryProperty(nameof(Files), "Files")]
public partial class ImageViewViewModel(IFileService fileService) : ObservableObject
{
    [ObservableProperty] 
    private FileItem? _file;

    [ObservableProperty] 
    private ObservableCollection<FileItem>? _files;

    [ObservableProperty]
    private ImageSource? _imageSource;

    partial void OnFileChanged(FileItem value)
    {
        ImageSource = ImageSource.FromUri(new Uri(File.Uri));
    }

    [RelayCommand]
    private async Task DeleteFileAsync()
    {
        var confirmed = await Shell.Current.DisplayAlert("Подтверждение", $"Удалить {File.Name}?", "Да", "Нет");
        if (!confirmed)
            return;

        var success = await fileService.DeleteFileAsync(File.Uri);
        if (success)
        {
            Files.Remove(File);
        }
        else
        {
            await Shell.Current.DisplayAlert("Ошибка", $"Не удалось удалить файл {File.Name}", "OK");
        }
    }
}
