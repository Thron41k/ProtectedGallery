using System.Collections.ObjectModel;
using Android.App;
using Android.Content;
using Android.Provider;
using AndroidX.DocumentFile.Provider;
using ProtectedGallery.Models;
using ProtectedGallery.Services.Interfaces;

[assembly: Dependency(typeof(ProtectedGallery.FileServiceAndroid))]
namespace ProtectedGallery;

public class FileServiceAndroid : IFileService, IActivityResultReceiver
{
    private const int RequestCodeOpenDocumentTree = 1000;
    private TaskCompletionSource<string?>? _tcsPickFolder;
    public ObservableCollection<FileItem> Files { get; set; } = [];

    public Task<string?> PickFolderAsync()
    {
        _tcsPickFolder = new TaskCompletionSource<string?>();
        var activity = Platform.CurrentActivity ?? throw new Exception("Current Activity is null");
        Intent intent = new(Intent.ActionOpenDocumentTree);
        intent.AddFlags(ActivityFlags.GrantReadUriPermission | ActivityFlags.GrantWriteUriPermission | ActivityFlags.GrantPersistableUriPermission);
        activity.StartActivityForResult(intent, RequestCodeOpenDocumentTree);
        return _tcsPickFolder.Task;
    }

    private void GetFilesAsync(string folderUri)
    {
        Files.Clear();
        var activity = Platform.CurrentActivity ?? throw new Exception("Current Activity is null");
        var contentResolver = activity.ContentResolver;
        var treeUri = Android.Net.Uri.Parse(folderUri);
        var childrenUri = DocumentsContract.BuildChildDocumentsUriUsingTree(treeUri, DocumentsContract.GetTreeDocumentId(treeUri));

        using var cursor = contentResolver?.Query(childrenUri!, null, null, null, null);
        if (cursor == null || !cursor.MoveToFirst()) return;

        var nameIndex = cursor.GetColumnIndex(DocumentsContract.Document.ColumnDisplayName);
        var idIndex = cursor.GetColumnIndex(DocumentsContract.Document.ColumnDocumentId);
        var dateIndex = cursor.GetColumnIndex(DocumentsContract.Document.ColumnLastModified);

        do
        {
            var name = nameIndex >= 0 ? cursor.GetString(nameIndex) : null;
            var documentId = idIndex >= 0 ? cursor.GetString(idIndex) : null;
            long? timestamp = dateIndex >= 0 ? cursor.GetLong(dateIndex) : null;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(documentId)) continue;

            var fileUri = DocumentsContract.BuildDocumentUriUsingTree(treeUri, documentId);
            var date = timestamp > 0
                ? (DateTime?)DateTimeOffset.FromUnixTimeMilliseconds(timestamp.Value).DateTime
                : null;

            Files.Add(new FileItem
            {
                Name = name,
                Uri = fileUri?.ToString(),
                DateTaken = date
            });

        } while (cursor.MoveToNext());
    }

    public Task<bool> DeleteFileAsync(string fileUri)
    {
        try
        {
            var context = Android.App.Application.Context;
            var uri = Android.Net.Uri.Parse(fileUri);
            var documentFile = DocumentFile.FromSingleUri(context, uri);
            if (documentFile == null)
            {
                return Task.FromResult(false);
            }
            var deleted = documentFile.Delete();
            if (!deleted) return Task.FromResult(deleted);
            Files.Remove(Files.First(f => f.Uri == fileUri));
            return Task.FromResult(deleted);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    public void OnActivityResult(int requestCode, Result resultCode, Intent? data)
    {
        if (requestCode != RequestCodeOpenDocumentTree) return;
        if (resultCode == Result.Ok && data != null)
        {
            var treeUri = data.Data!;
            var activity = Platform.CurrentActivity!;
            activity.ContentResolver?.TakePersistableUriPermission(treeUri,
                ActivityFlags.GrantReadUriPermission | ActivityFlags.GrantWriteUriPermission);
            GetFilesAsync(treeUri.ToString()!);
            _tcsPickFolder?.TrySetResult(treeUri.ToString());
        }
        else
        {
            _tcsPickFolder?.TrySetResult(null);
        }
    }
}
