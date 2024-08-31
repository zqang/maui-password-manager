using PasswordManager.Services.Settings;

namespace PasswordManager.Services;

public class MauiNavigationService : INavigationService
{
    private readonly ISettingsService _settingsService;

    public MauiNavigationService(ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    public Task InitializeAsync() =>
        NavigateToAsync("//Main/Passwords");

    public Task NavigateToAsync(string route, IDictionary<string, object> routeParameters = null)
    {
        var shellNavigation = new ShellNavigationState(route);

        return routeParameters != null
            ? Shell.Current.GoToAsync(shellNavigation, routeParameters)
            : Shell.Current.GoToAsync(shellNavigation);
    }

    public Task PopAsync() =>
        Shell.Current.GoToAsync("..");
}
