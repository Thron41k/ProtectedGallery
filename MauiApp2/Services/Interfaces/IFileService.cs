using System.Collections.ObjectModel;
using MauiApp2.Models;

namespace MauiApp2.Services.Interfaces;

public interface IFileService
{
    ObservableCollection<FileItem> Files { get; set; }
    Task<string?> PickFolderAsync();
    Task<bool> DeleteFileAsync(string fileUri);
}