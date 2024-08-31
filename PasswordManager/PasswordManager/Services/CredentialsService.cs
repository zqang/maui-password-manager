using PasswordManager;
using PasswordManager.Helpers;
using PasswordManager.Models;
using PasswordManager.Services.RequestProvider;

namespace PasswordManager.Services;

public class CredentialsService : ICredentialsService
{
    private const string ApiUrlBase = "Credential";
    private readonly IRequestProvider _requestProvider;

    public CredentialsService(IRequestProvider requestProvider)
    {
        _requestProvider = requestProvider;
    }
    
    public async Task<IEnumerable<Credential>> GetCredentialsAsync()
    {
        var uri = UriHelper.CombineUri(GlobalSetting.Instance.GatewayCredentialEndpoint, $"{ApiUrlBase}");

        IEnumerable<Credential> credentials = await _requestProvider.GetAsync<IEnumerable<Credential>>(uri).ConfigureAwait(false);

        return credentials;
    }

    public async Task<Guid> CreateCredentialAsync(Credential credential)
    {
        var uri = UriHelper.CombineUri(GlobalSetting.Instance.GatewayCredentialEndpoint, $"{ApiUrlBase}");

        var credentialId = await _requestProvider.PostAsync<Guid, Credential>(uri, credential).ConfigureAwait(false);

        return credentialId;
    }

    public async Task<Guid> UpdateCredentialAsync(Guid id, Credential credential)
    {
        var uri = UriHelper.CombineUri(GlobalSetting.Instance.GatewayCredentialEndpoint, $"{ApiUrlBase}/{id}");

        var credentialId = await _requestProvider.PutAsync<Guid, Credential>(uri, credential).ConfigureAwait(false);

        return credentialId;
    }

    public async Task DeleteCredentialAsync(Guid id)
    {
        var uri = UriHelper.CombineUri(GlobalSetting.Instance.GatewayCredentialEndpoint, $"{ApiUrlBase}/{id}");

        await _requestProvider.DeleteAsync(uri).ConfigureAwait(false);
    }
}