using PasswordManager.Services;
using PasswordManager.ViewModels.Base;
using PasswordManager.Models;
using PasswordManager.Services.AppEnvironment;
using PasswordManager.Services.Settings;
using PasswordManager.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace PasswordManager.ViewModels;

public partial class PasswordsViewModel : ViewModelBase, INotifyPropertyChanged
{
    private readonly IAppEnvironmentService _appEnvironmentService;
    private readonly ISettingsService _settingsService;
    private readonly ObservableCollectionEx<Credential> _credentials = new();
    private string _searchText;
    private ObservableCollection<Credential> _filteredCredentials;

    private bool _initialized;
    
    [ObservableProperty]
    private Credential _selectedCredential;
    
    [ObservableProperty]
    private int _badgeCount;
    
    public IReadOnlyList<Credential> Credentials => _credentials;

    public PasswordsViewModel(IAppEnvironmentService appEnvironmentService,
        INavigationService navigationService, ISettingsService settingsService)
        : base(navigationService)
    {
        _appEnvironmentService = appEnvironmentService;
        _settingsService = settingsService;
    }
    
    public override async Task InitializeAsync()
    {
        await RefreshCredentials();
        FilteredCredentials = new ObservableCollection<Credential>(_credentials); // Initialize FilteredCredentials
    }

    [RelayCommand]
    public async Task RefreshCredentials()
    {
        await IsBusyFor(async () =>
        {
            var credentials = await _appEnvironmentService.CredentialsService.GetCredentialsAsync();
            _credentials.Clear(); // Clear existing credentials
            foreach (var credential in credentials)
            {
                _credentials.Add(credential); // Add new credentials
            }
            BadgeCount = _credentials.Count;
            FilteredCredentials = new ObservableCollection<Credential>(_credentials); // Update filtered list
        });
    }
    
    [RelayCommand]
    private async Task AddCredential()
    {
        await NavigationService.NavigateToAsync(nameof(AddPasswordPage));
    }

    [RelayCommand]
    private async Task EditCredential(Credential credential)
    {
        if (credential != null)
        {
            var parameters = new Dictionary<string, object>
            {
                { nameof(EditPasswordViewModel.CredentialId), credential.id }
            };
            await NavigationService.NavigateToAsync(nameof(EditPasswordPage), parameters);
        }
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (_searchText != value)
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilterCredentials();
            }
        }
    }

    public ObservableCollection<Credential> FilteredCredentials
    {
        get => _filteredCredentials;
        private set
        {
            _filteredCredentials = value;
            OnPropertyChanged(nameof(FilteredCredentials));
        }
    }

    private void FilterCredentials()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            FilteredCredentials = new ObservableCollection<Credential>(_credentials);
        }
        else
        {
            var filtered = _credentials
                .Where(c => c.Domain.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) // Adjust property as needed
                .ToList();
            FilteredCredentials = new ObservableCollection<Credential>(filtered);
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}