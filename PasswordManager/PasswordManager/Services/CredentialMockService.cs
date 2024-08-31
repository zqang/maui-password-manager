using PasswordManager.Models;

namespace PasswordManager.Services;

public class CredentialMockService : ICredentialsService
{

    private IEnumerable<Credential> MockCredential =
        new[]
        {
            new Credential { id = new Guid(), PictureUri = "fake_product_01.png", Domain = "facebook.com", UserId = "zqang", Password = "alvin1406"},
            new Credential { id = new Guid(), PictureUri = "fake_product_02.png", Domain = "gmail.com", UserId = "imaazq", Password = "alvin1406"},
            new Credential { id = new Guid(), PictureUri = "fake_product_03.png", Domain = "apple.com", UserId = "alvinang", Password = "alvin1406"}
        };

    public async Task<IEnumerable<Credential>> GetCredentialsAsync()
    {
        await Task.Delay(10);

        return MockCredential;
    }

    public async Task<Guid> CreateCredentialAsync(Credential credential)
    {
        var newMock = MockCredential.ToList();
        newMock.Add(credential);
        MockCredential = newMock.AsEnumerable();

        return credential.id;
    }

    public async Task<Guid> UpdateCredentialAsync(Guid id, Credential credential)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteCredentialAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
