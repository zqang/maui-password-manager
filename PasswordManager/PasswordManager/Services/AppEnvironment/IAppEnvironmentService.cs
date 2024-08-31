
namespace PasswordManager.Services.AppEnvironment;

public interface IAppEnvironmentService
{
    ICredentialsService CredentialsService { get; }

    void UpdateDependencies(bool useMockServices);
}
