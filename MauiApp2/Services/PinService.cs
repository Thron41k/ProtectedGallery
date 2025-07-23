using MauiApp2.Services.Interfaces;

namespace MauiApp2.Services;

public class PinService : IPinService
{
    private const string PinKey = "UserPin";

    public async Task<bool> IsPinSetAsync() =>
        await SecureStorage.Default.GetAsync(PinKey) is not null;

    public async Task SetPinAsync(string? pin) =>
        await SecureStorage.Default.SetAsync(PinKey, pin!);

    public async Task<bool> ValidatePinAsync(string? input)
    {
        var stored = await SecureStorage.Default.GetAsync(PinKey);
        return stored == input;
    }
}