using PasswordManager.Models;
using System.Net;

namespace PasswordManager.Services;

public class CredentialMockService : ICredentialsService
{

    private static List<Credential> MockCredential = new List<Credential>
    {
        new Credential { id = Guid.NewGuid(), PictureUri = "fake_product_01.png", Domain = "facebook.com", UserId = "zqang", Password = "alvin1406"},
        new Credential { id = Guid.NewGuid(), PictureUri = "fake_product_02.png", Domain = "gmail.com", UserId = "imaazq", Password = "alvin1406"},
        new Credential { id = Guid.NewGuid(), PictureUri = "fake_product_03.png", Domain = "apple.com", UserId = "alvinang", Password = "alvin1406"}
    };

    public async Task<IEnumerable<Credential>> GetCredentialsAsync()
    {
        await Task.Delay(10); // Simulate network delay
        return MockCredential;
    }

    public async Task<Credential> GetCredentialAsync(Guid id)
    {
        await Task.Delay(10); // Simulate network delay
        return MockCredential.FirstOrDefault(c => c.id == id);
    }

    public async Task<Guid> CreateCredentialAsync(Credential credential)
    {
        await Task.Delay(10); // Simulate network delay
        if (credential.id == Guid.Empty)
        {
            credential.id = Guid.NewGuid();
        }
        MockCredential.Add(credential);
        return credential.id;
    }

    public async Task<Guid> UpdateCredentialAsync(Guid id, Credential credential)
    {
        await Task.Delay(10); // Simulate network delay
        var existingCredential = MockCredential.FirstOrDefault(c => c.id == id);
        if (existingCredential != null)
        {
            // Remove the existing credential
            MockCredential.Remove(existingCredential);
            
            // Add the updated credential
            MockCredential.Add(credential);
            
            return credential.id;
        }
        throw new Exception("Credential not found");
    }

    public async Task DeleteCredentialAsync(Guid id)
    {
        await Task.Delay(10); // Simulate network delay
        var credentialToRemove = MockCredential.FirstOrDefault(c => c.id == id);
        if (credentialToRemove != null)
        {
            MockCredential.Remove(credentialToRemove);
        }
    }

    public Task ClearAllCredentialsAsync()
    {
        MockCredential.Clear();
        return Task.CompletedTask;
    }
}
