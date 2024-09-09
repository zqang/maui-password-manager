using PasswordManager;
using PasswordManager.Helpers;
using PasswordManager.Models;
using PasswordManager.Services.RequestProvider;
using PasswordManager.Services.Settings;

namespace PasswordManager.Services;

public class CredentialsService : ICredentialsService
{
    private const string ApiUrlBase = "Credential";
    private readonly IRequestProvider _requestProvider;
    private readonly LocalStorageService _localStorageService;
    private readonly ISettingsService _settingsService;

    public CredentialsService(IRequestProvider requestProvider, LocalStorageService localStorageService, ISettingsService settingsService)
    {
        _requestProvider = requestProvider;
        _localStorageService = localStorageService;
        _settingsService = settingsService;
    }
    
    public async Task<IEnumerable<Credential>> GetCredentialsAsync()
    {
        if (IsApiEnabled())
        {
            var uri = UriHelper.CombineUri(GlobalSetting.Instance.GatewayCredentialEndpoint, $"{ApiUrlBase}");
            IEnumerable<Credential> credentials = await _requestProvider.GetAsync<IEnumerable<Credential>>(uri).ConfigureAwait(false);
            await SyncLocalStorage(credentials);
            return credentials;
        }
        return await _localStorageService.GetCredentialsAsync();
    }

    public async Task<Credential> GetCredentialAsync(Guid id)
    {
        if (IsApiEnabled())
        {
            var uri = UriHelper.CombineUri(GlobalSetting.Instance.GatewayCredentialEndpoint, $"{ApiUrlBase}/{id}");
            Credential credential = await _requestProvider.GetAsync<Credential>(uri).ConfigureAwait(false);
            await _localStorageService.SaveCredentialAsync(credential);
            return credential;
        }
        return await _localStorageService.GetCredentialAsync(id);
    }

    public async Task<Guid> CreateCredentialAsync(Credential credential)
    {
        if (credential.id == Guid.Empty)
        {
            credential.id = Guid.NewGuid();
        }

        if (IsApiEnabled())
        {
            var uri = UriHelper.CombineUri(GlobalSetting.Instance.GatewayCredentialEndpoint, $"{ApiUrlBase}");
            await _requestProvider.PostAsync<Guid, Credential>(uri, credential).ConfigureAwait(false);
        }
        await _localStorageService.SaveCredentialAsync(credential);
        return credential.id;
    }

    public async Task<Guid> UpdateCredentialAsync(Guid id, Credential credential)
    {
        credential.id = id;
        if (IsApiEnabled())
        {
            var uri = UriHelper.CombineUri(GlobalSetting.Instance.GatewayCredentialEndpoint, $"{ApiUrlBase}/{id}");
            await _requestProvider.PutAsync<Guid, Credential>(uri, credential).ConfigureAwait(false);
        }
        await _localStorageService.SaveCredentialAsync(credential);
        return id;
    }

    public async Task DeleteCredentialAsync(Guid id)
    {
        if (IsApiEnabled())
        {
            var uri = UriHelper.CombineUri(GlobalSetting.Instance.GatewayCredentialEndpoint, $"{ApiUrlBase}/{id}");
            await _requestProvider.DeleteAsync(uri).ConfigureAwait(false);
        }
        var credential = await _localStorageService.GetCredentialAsync(id);
        if (credential != null)
        {
            await _localStorageService.DeleteCredentialAsync(credential);
        }
    }

    public async Task ClearAllCredentialsAsync()
    {
        if (IsApiEnabled())
        {
            var uri = UriHelper.CombineUri(GlobalSetting.Instance.GatewayCredentialEndpoint, $"{ApiUrlBase}");
            await _requestProvider.DeleteAsync(uri).ConfigureAwait(false);
        }
        await _localStorageService.DeleteAllCredentialsAsync();
    }

    private bool IsApiEnabled()
    {
        return !string.IsNullOrEmpty(_settingsService.CredentialEndpointBase);
    }

    private async Task SyncLocalStorage(IEnumerable<Credential> apiCredentials)
    {
        var localCredentials = await _localStorageService.GetCredentialsAsync();
        var credentialsToDelete = localCredentials.Where(lc => !apiCredentials.Any(ac => ac.id == lc.id));
        var credentialsToAddOrUpdate = apiCredentials.Where(ac => !localCredentials.Any(lc => lc.id == ac.id && lc.Password == ac.Password));

        foreach (var credential in credentialsToDelete)
        {
            await _localStorageService.DeleteCredentialAsync(credential);
        }

        foreach (var credential in credentialsToAddOrUpdate)
        {
            await _localStorageService.SaveCredentialAsync(credential);
        }
    }
}