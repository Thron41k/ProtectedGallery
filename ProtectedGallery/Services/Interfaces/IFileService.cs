using System.Collections.ObjectModel;
using ProtectedGallery.Models;

namespace ProtectedGallery.Services.Interfaces;

public interface IFileService
{
    ObservableCollection<FileItem> Files { get; set; }
    Task<string?> PickFolderAsync();
    Task<bool> DeleteFileAsync(string fileUri);
}