namespace MauiApp2.Services.Interfaces;

public interface IPinService
{
    Task<bool> IsPinSetAsync();
    Task SetPinAsync(string? pin);
    Task<bool> ValidatePinAsync(string? input);
}