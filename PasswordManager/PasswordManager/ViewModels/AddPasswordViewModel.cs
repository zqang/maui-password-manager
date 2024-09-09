using PasswordManager.Services;
using PasswordManager.ViewModels.Base;
using PasswordManager.Models;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace PasswordManager.ViewModels;

public partial class AddPasswordViewModel : ViewModelBase
{
    private readonly ICredentialsService _credentialsService;

    [ObservableProperty]
    private string _domain;

    [ObservableProperty]
    private string _username;

    [ObservableProperty]
    private string _password;

    [ObservableProperty]
    private string _note;

    public ObservableCollection<PasswordItem> Passwords { get; set; }

    public AddPasswordViewModel(INavigationService navigationService, ICredentialsService credentialsService) 
        : base(navigationService)
    {
        _credentialsService = credentialsService;
        Passwords = new ObservableCollection<PasswordItem>
        {
            new PasswordItem { URL = "http://103.233.3.93:8080", IconSource = "network_icon.png" },
            new PasswordItem { URL = "http://127.0.0.1:8000", IconSource = "network_icon.png" },
            new PasswordItem { URL = "http://192.168.25.1", IconSource = "network_icon.png" },
            new PasswordItem { URL = "account.bbk.com", IconSource = "android_icon.png" },
            new PasswordItem { URL = "adobe.com", IconSource = "adobe_icon.png" },
            new PasswordItem { URL = "Airbnb", IconSource = "airbnb_icon.png" },
            new PasswordItem { URL = "alipay.com", IconSource = "alipay_icon.png" },
            new PasswordItem { URL = "amazon.com", IconSource = "amazon_icon.png" }
        };
    }

    [RelayCommand]
    private async Task SaveCredentialAsync()
    {
        if (string.IsNullOrWhiteSpace(Domain) || string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Please fill in all required fields.", "OK");
            return;
        }

        var newCredential = new Credential
        {
            id = Guid.NewGuid(), // Generate a new GUID for the credential
            Domain = Domain,
            UserId = Username,
            Password = Password,
            Secret = Note
        };

        try
        {
            var credentialId = await _credentialsService.CreateCredentialAsync(newCredential);
            await Application.Current.MainPage.DisplayAlert("Success", "Password saved successfully.", "OK");
            await NavigationService.PopAsync();
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Failed to save password: {ex.Message}", "OK");
        }
    }

    public class PasswordItem
    {
        public string URL { get; set; }
        public string IconSource { get; set; }
    }
}