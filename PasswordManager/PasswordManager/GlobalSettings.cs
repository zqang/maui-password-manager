namespace PasswordManager;

/// <summary>
/// Refer from eshop ClientApp project GlobalSettings.cs
/// </summary>

public class GlobalSetting
{
    public const string MockTag = "Mock";
    public const string DefaultEndpoint = ""; // Changed to empty string

    private string _baseGatewayCredentialEndpoint;

    public GlobalSetting()
    {
        BaseGatewayCredentialEndpoint = DefaultEndpoint;
    }

    public static GlobalSetting Instance { get; } = new GlobalSetting();

    public string BaseGatewayCredentialEndpoint
    {
        get => _baseGatewayCredentialEndpoint;
        set
        {
            _baseGatewayCredentialEndpoint = value;
            UpdateGatewayCredentialEndpoint(_baseGatewayCredentialEndpoint);
        }
    }

    public string GatewayCredentialEndpoint { get; set; }

    private void UpdateGatewayCredentialEndpoint(string endpoint)
    {
        GatewayCredentialEndpoint = string.IsNullOrEmpty(endpoint) ? null : endpoint;
    }
}
