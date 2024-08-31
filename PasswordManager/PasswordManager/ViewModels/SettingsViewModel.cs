using PasswordManager.Services;
using PasswordManager.ViewModels.Base;

namespace PasswordManager.ViewModels;

public class SettingsViewModel : ViewModelBase
{
    public SettingsViewModel(INavigationService navigationService) : base(navigationService)
    {
    }
}