using Android.Content;
using Android.Database;
using Android.Provider;
using Android.App;
using AndroidX.DocumentFile.Provider;
using MauiApp2.Models;
using MauiApp2.Services;
using MauiApp2.Services.Interfaces;

[assembly: Microsoft.Maui.Controls.Dependency(typeof(MauiApp2.Platforms.Android.FileService_Android))]

namespace MauiApp2.Platforms.Android;

public class FileService_Android : IFileService, IActivityResultReceiver
{
    private const int REQUEST_CODE_OPEN_DOCUMENT_TREE = 1000;
    private TaskCompletionSource<string?>? _tcsPickFolder;

    public Task<string?> PickFolderAsync()
    {
        _tcsPickFolder = new TaskCompletionSource<string?>();
        var activity = Platform.CurrentActivity ?? throw new Exception("Current Activity is null");

        Intent intent = new(Intent.ActionOpenDocumentTree);
        intent.AddFlags(ActivityFlags.GrantReadUriPermission | ActivityFlags.GrantWriteUriPermission | ActivityFlags.GrantPersistableUriPermission);

        activity.StartActivityForResult(intent, REQUEST_CODE_OPEN_DOCUMENT_TREE);
        return _tcsPickFolder.Task;
    }

    public Task<List<FileItem>> GetFilesAsync(string folderUri)
    {
        var files = new List<FileItem>();
        var activity = Platform.CurrentActivity ?? throw new Exception("Current Activity is null");
        var contentResolver = activity.ContentResolver;
        var treeUri = global::Android.Net.Uri.Parse(folderUri);

        var childrenUri = DocumentsContract.BuildChildDocumentsUriUsingTree(treeUri, DocumentsContract.GetTreeDocumentId(treeUri));

        using (ICursor? cursor = contentResolver.Query(childrenUri, new string[] { "display_name", "document_id" }, null, null, null))
        {
            if (cursor != null && cursor.MoveToFirst())
            {
                do
                {
                    string name = cursor.GetString(0);
                    string documentId = cursor.GetString(1);
                    var fileUri = DocumentsContract.BuildDocumentUriUsingTree(treeUri, documentId);

                    files.Add(new FileItem { Name = name, Uri = fileUri.ToString() });
                }
                while (cursor.MoveToNext());
            }
        }

        return Task.FromResult(files);
    }

    public async Task<bool> DeleteFileAsync(string fileUri)
    {
        try
        {
            var context = global::Android.App.Application.Context;
            var uri = global::Android.Net.Uri.Parse(fileUri);

            // Получаем DocumentFile по Uri
            var documentFile = DocumentFile.FromSingleUri(context, uri);

            if (documentFile == null)
            {
                System.Diagnostics.Debug.WriteLine("DeleteFileAsync: DocumentFile is null");
                return false;
            }

            // Удаляем файл через DocumentFile API
            bool deleted = documentFile.Delete();

            System.Diagnostics.Debug.WriteLine($"DeleteFileAsync: deleted = {deleted}");

            return deleted;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"DeleteFileAsync exception: {ex}");
            return false;
        }
    }

    public void OnActivityResult(int requestCode, Result resultCode, Intent? data)
    {
        if (requestCode == REQUEST_CODE_OPEN_DOCUMENT_TREE)
        {
            if (resultCode == Result.Ok && data != null)
            {
                var treeUri = data.Data!;
                var activity = Platform.CurrentActivity!;
                activity.ContentResolver.TakePersistableUriPermission(treeUri,
                    ActivityFlags.GrantReadUriPermission | ActivityFlags.GrantWriteUriPermission);

                _tcsPickFolder?.TrySetResult(treeUri.ToString());
            }
            else
            {
                _tcsPickFolder?.TrySetResult(null);
            }
        }
    }
}
