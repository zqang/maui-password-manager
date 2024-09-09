namespace PasswordManager.Services.Settings;

public class SettingsService : ISettingsService
{
    #region Setting Constants

    private const string IdUseMocks = "use_mocks";
    private const string IdCredentialBase = "url_base";
    private readonly bool UseMocksDefault = true;
    private readonly string UrlCredentialDefault = GlobalSetting.Instance.BaseGatewayCredentialEndpoint;
    #endregion

    #region Settings Properties
    
    public bool UseMocks
    {
        get => Preferences.Get(IdUseMocks, UseMocksDefault);
        set => Preferences.Set(IdUseMocks, value);
    }

    public string CredentialEndpointBase
    {
        get => Preferences.Get(IdCredentialBase, string.Empty);
        set
        {
            Preferences.Set(IdCredentialBase, value);
            GlobalSetting.Instance.BaseGatewayCredentialEndpoint = value;
        }
    }

    #endregion
}
