using MauiApp2.Models;

namespace MauiApp2.Services.Interfaces
{
    public interface IFileService
    {
        Task<string?> PickFolderAsync();
        Task<List<FileItem>> GetFilesAsync(string folderUri);
        Task<bool> DeleteFileAsync(string fileUri);
    }
}
