namespace MauiApp2.Helpers;

public static class UiHelpers
{
    public static Page? GetCurrentPage()
    {
        return Application.Current?.Windows.FirstOrDefault()?.Page;
    }

    public static async Task ShowAlert(string title, string message, string cancel = "OK")
    {
        var page = GetCurrentPage();
        if (page != null)
            await page.DisplayAlert(title, message, cancel);
    }

    public static async Task<bool> ShowConfirm(string title, string message, string accept = "Да", string cancel = "Нет")
    {
        var page = GetCurrentPage();
        if (page != null)
            return await page.DisplayAlert(title, message, accept, cancel);
        return false;
    }
}