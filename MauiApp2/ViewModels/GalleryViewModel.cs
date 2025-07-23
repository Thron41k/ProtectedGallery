using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp2.Models;
using MauiApp2.Services.Interfaces;
using MauiApp2.Views;

namespace MauiApp2.ViewModels;

public partial class GalleryViewModel : ObservableObject
{
    private readonly IFileService _fileService;

    [ObservableProperty]
    private ObservableCollection<FileItem> _files = [];

    [ObservableProperty]
    private FileItem? _selectedImage;

    private string? _currentFolderUri;

    public GalleryViewModel(IFileService fileService)
    {
        _fileService = fileService;
    }

    [RelayCommand]
    public async Task PickFolder()
    {
        _currentFolderUri = await _fileService.PickFolderAsync();
        if (!string.IsNullOrEmpty(_currentFolderUri))
        {
            var items = await _fileService.GetFilesAsync(_currentFolderUri);
            Files.Clear();
            foreach (var f in items)
            {
                Files.Add(f);
            }

        }
    }

    [RelayCommand]
    public async Task DeleteFile(FileItem file)
    {
        if (file == null) return;

        bool success = await _fileService.DeleteFileAsync(file.Uri);
        if (success)
            Files.Remove(file);
        else
            await App.Current.MainPage.DisplayAlert("Ошибка", $"Не удалось удалить файл {file.Name}", "OK");
    }

    [RelayCommand]
    public async Task OpenFile(FileItem file)
    {
        if (file == null)
            return;
        await Shell.Current.GoToAsync("ImageViewPage", new Dictionary<string, object>
        {
            ["File"] = file
        });
    }
}
