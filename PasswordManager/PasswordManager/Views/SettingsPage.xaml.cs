using Plugin.Fingerprint;

namespace PasswordManager.Views;

public partial class SettingsPage : ContentPage
{
    private SettingsViewModel? _viewModel => BindingContext as SettingsViewModel;

    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
#if  WINDOWS
        BiometricSwitch.IsEnabled = false;
#else
        CheckBiometricAvailability();
#endif
        
    }

    private async void CheckBiometricAvailability()
    {
        var availability = await CrossFingerprint.Current.IsAvailableAsync();
        if (!availability)
        {
            BiometricSwitch.IsEnabled = false;
            await DisplayAlert("Biometric Unavailable", "Biometric authentication is not available on this device.", "OK");
        }
    }

    private async void OnBiometricSwitchToggled(object sender, ToggledEventArgs e)
    {
        var result = await _viewModel!.ToggleBiometricAsync(e.Value);
        if(result == "Success")
            await DisplayAlert("Success", "Biometric authentication enabled.", "OK");
        else if (result == "Failed")
        {
            BiometricSwitch.IsToggled = false;
            await DisplayAlert("Failed", "Biometric authentication could not be enabled.", "OK");
        }
        else
        {
            await DisplayAlert("Disabled", "Biometric authentication disabled.", "OK");
        }
    }

    private void OnAutoLockOptionChanged(object sender, EventArgs e)
    {
        if (sender is Picker picker)
        {
            _viewModel!.UpdateAutoLockOption(picker.SelectedItem.ToString());
        }
    }

}