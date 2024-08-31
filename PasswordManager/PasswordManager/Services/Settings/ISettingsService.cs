namespace PasswordManager.Services.Settings;

public interface ISettingsService
{
    bool UseMocks { get; set; }
    string CredentialEndpointBase { get; set; }
}
