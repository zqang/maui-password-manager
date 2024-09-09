using PasswordManager.Models;

namespace PasswordManager.Services;

public interface ICredentialsService
{
    public Task<IEnumerable<Credential>> GetCredentialsAsync();
    public Task<Credential> GetCredentialAsync(Guid id);
    public Task<Guid> CreateCredentialAsync(Credential credential);
    public Task<Guid> UpdateCredentialAsync(Guid id, Credential credential);
    public Task DeleteCredentialAsync(Guid id);
    public Task ClearAllCredentialsAsync();
}