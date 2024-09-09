using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PasswordManager.Models;
using PasswordManager.Services;
using PasswordManager.ViewModels.Base;
using PasswordManager.Views;

namespace PasswordManager.ViewModels;

[QueryProperty(nameof(CredentialId), nameof(CredentialId))]
public partial class EditPasswordViewModel : ViewModelBase
{
    private readonly ICredentialsService _credentialsService;

    [ObservableProperty]
    private Guid _credentialId;

    [ObservableProperty]
    private string _domain;

    [ObservableProperty]
    private string _username;

    [ObservableProperty]
    private string _password;

    [ObservableProperty]
    private string _note;

    public EditPasswordViewModel(INavigationService navigationService, ICredentialsService credentialsService) 
        : base(navigationService)
    {
        _credentialsService = credentialsService;
    }

    public override async Task InitializeAsync()
    {
        var credential = await _credentialsService.GetCredentialAsync(CredentialId);
        Domain = credential.Domain;
        Username = credential.UserId;
        Password = credential.Password;
        Note = credential.Secret;
    }

    [RelayCommand]
    private async Task UpdateCredentialAsync()
    {
        if (string.IsNullOrWhiteSpace(Domain) || string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Please fill in all required fields.", "OK");
            return;
        }

        var updatedCredential = new Credential
        {
            id = CredentialId,
            Domain = Domain,
            UserId = Username,
            Password = Password,
            Secret = Note
        };

        try
        {
            await _credentialsService.UpdateCredentialAsync(CredentialId, updatedCredential);
            await Application.Current.MainPage.DisplayAlert("Success", "Password updated successfully.", "OK");
            await NavigationService.PopAsync();
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Failed to update password: {ex.Message}", "OK");
        }
    }
}