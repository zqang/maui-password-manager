using Microsoft.Maui.Controls;
using PasswordManager.Services;
using PasswordManager.Services.SecureStorages;
using PasswordManager.Services.Settings;
using PasswordManager.ViewModels.Base;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;

namespace PasswordManager.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly ISecureStorageService _secureStorageService;
    private readonly ICredentialsService _credentialService;
    private readonly ISettingsService _settingService;


    [ObservableProperty]
    private bool _isBiometricEnabled;

    [ObservableProperty]
    private string _selectedAutoLockOption;

    [ObservableProperty]
    private string _credentialEndpoint;

    public List<string> AutoLockOptions { get; } = new List<string> { "Immediately", "After 1 minute", "After 5 minutes", "After 10 minutes", "After 30 minutes", "Never" };

    public SettingsViewModel(ISecureStorageService secureStorageService, ICredentialsService credentialService, ISettingsService settingService, INavigationService navigation) : base(navigation)
    {
        _secureStorageService = secureStorageService;
        _credentialService = credentialService;
        _settingService = settingService;

        IsBiometricEnabled = _secureStorageService.GetBiometricSetting();
        SelectedAutoLockOption= _secureStorageService.GetAutoLockSetting() ?? "After 5 minutes";
    }

    public async Task<string> ToggleBiometricAsync(bool newValue)
    {
        if (newValue)
        {
            var result = await CrossFingerprint.Current.AuthenticateAsync(new AuthenticationRequestConfiguration("Enable Biometric", "Please authenticate to enable biometric login"));
            if (result.Authenticated)
            {
                _secureStorageService.SetBiometricSetting(true);
                IsBiometricEnabled = true;
                return "Success";
            }
            return "Failed";
        }
        else
        {
            _secureStorageService.SetBiometricSetting(false);
            IsBiometricEnabled = false;
            return "Disabled";
        }
    }


    public void UpdateAutoLockOption(string newValue)
    {
        SelectedAutoLockOption = newValue;
        _secureStorageService.SetAutoLockSetting(newValue);
    }

    [RelayCommand]
    private async Task ChangeMasterPasswordAsync()
    {
        // This method will be called when the Change Master Password button is clicked
        // You'll need to implement the logic to change the master password here
        // For example:
        string currentPassword = await Application.Current.MainPage.DisplayPromptAsync("Current Password", "Enter your current master password");
        if (string.IsNullOrEmpty(currentPassword))
            return;

        if (!_secureStorageService.VerifyMasterPassword(currentPassword))
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Incorrect current password.", "OK");
            return;
        }

        string newPassword = await Application.Current.MainPage.DisplayPromptAsync("New Password", "Enter your new master password");
        if (string.IsNullOrEmpty(newPassword))
            return;

        string confirmPassword = await Application.Current.MainPage.DisplayPromptAsync("Confirm Password", "Confirm your new master password");
        if (newPassword != confirmPassword)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "New passwords do not match.", "OK");
            return;
        }

        _secureStorageService.ChangeMasterPassword(currentPassword, newPassword);
        await Application.Current.MainPage.DisplayAlert("Success", "Master password changed successfully.", "OK");
    }

    [RelayCommand]
    private async Task ClearAllDataAsync()
    {
        bool confirm = await Application.Current.MainPage.DisplayAlert("Warning", "Are you sure you want to clear all data? This action cannot be undone.", "Yes", "No");
        if (confirm)
        {
            await _credentialService.ClearAllCredentialsAsync();
            _secureStorageService.ClearAllSecureData();
            await Application.Current.MainPage.DisplayAlert("Success", "All data has been cleared.", "OK");
        }
    }

    [RelayCommand]
    private void SaveCredentialEndpoint()
    {
        _settingService.CredentialEndpointBase = CredentialEndpoint;
    }
}
