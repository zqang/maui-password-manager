
namespace PasswordManager.Services.AppEnvironment;

public class AppEnvironmentService : IAppEnvironmentService
{
    private readonly ICredentialsService _mockCredentialService;
    private readonly ICredentialsService _credentialService;
    
    public ICredentialsService CredentialsService { get; private set; }

    public AppEnvironmentService(
        ICredentialsService mockCredentialService, ICredentialsService credentialService)
    {
        _mockCredentialService = mockCredentialService;
        _credentialService = credentialService;
    }

    public void UpdateDependencies(bool useMockServices)
    {
        if (useMockServices)
        {
            CredentialsService = _mockCredentialService;
        }
        else
        {
            CredentialsService = _credentialService;
        }
    }
}

