using PasswordManager.Services;
using PasswordManager.ViewModels.Base;
using PasswordManager.Models;
using PasswordManager.Services.AppEnvironment;
using PasswordManager.Services.Settings;

namespace PasswordManager.ViewModels;

public partial class PasswordsViewModel : ViewModelBase
{
    private readonly IAppEnvironmentService _appEnvironmentService;
    private readonly ISettingsService _settingsService;
    private readonly ObservableCollectionEx<Credential> _credentials = new();

    private bool _initialized;
    
    [ObservableProperty]
    private Credential _selectedCredential;
    
    [ObservableProperty]
    private int _badgeCount;
    
    public IReadOnlyList<Credential> Credentials => _credentials;

    public PasswordsViewModel( IAppEnvironmentService appEnvironmentService,
        INavigationService navigationService, ISettingsService settingsService)
        : base(navigationService)
    {
        _appEnvironmentService = appEnvironmentService;
        _settingsService = settingsService;

    }
    
    public override async Task InitializeAsync()
    {
        if (_initialized)
            return;

        _initialized = true;
        await IsBusyFor(
            async () =>
            {
                // Get Credentials
                var credentials = await _appEnvironmentService.CredentialsService.GetCredentialsAsync();
                _credentials.ReloadData(credentials);
            });
    }
    
    [RelayCommand]
    private async Task AddCredential()
    {
        await NavigationService.NavigateToAsync("AddPassword");
    }


}