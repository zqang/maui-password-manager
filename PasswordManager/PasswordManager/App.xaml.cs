using System.Diagnostics;
using PasswordManager.Services;
using PasswordManager.Services.AppEnvironment;
using PasswordManager.Services.Settings;
using PasswordManager.Services.Theme;

namespace PasswordManager;

public partial class App : Application
{
	private readonly ISettingsService _settingsService;
	private readonly IAppEnvironmentService _appEnvironmentService;
	private readonly INavigationService _navigationService;
	private readonly ITheme _theme;
	
    public App(
        ISettingsService settingsService, IAppEnvironmentService appEnvironmentService,
        INavigationService navigationService, ITheme theme)
    {
        _settingsService = settingsService;
        _appEnvironmentService = appEnvironmentService;
        _navigationService = navigationService;
        _theme = theme;

        InitializeComponent();

        InitApp();

        MainPage = new AppShell(navigationService);


        Application.Current.UserAppTheme = AppTheme.Light;
    }

    private void InitApp()
    {
        if (VersionTracking.IsFirstLaunchEver)
        {
            _settingsService.UseMocks = true;
        }

        if (!_settingsService.UseMocks)
        {
            _appEnvironmentService.UpdateDependencies(_settingsService.UseMocks);
        }
    }

    protected override void OnSleep()
    {
        SetStatusBar();
        RequestedThemeChanged -= App_RequestedThemeChanged;
    }

    protected override void OnResume()
    {
        SetStatusBar();
        RequestedThemeChanged += App_RequestedThemeChanged;
    }

    private void App_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
    {
        Dispatcher.Dispatch(() => SetStatusBar());
    }

    void SetStatusBar()
    {
        var nav = Current.MainPage as NavigationPage;

        if (Current.RequestedTheme == AppTheme.Dark)
        {
            _theme?.SetStatusBarColor(Colors.Black, false);
            if (nav != null)
            {
                nav.BarBackgroundColor = Colors.Black;
                nav.BarTextColor = Colors.White;
            }
        }
        else
        {
            _theme?.SetStatusBarColor(Colors.White, true);
            if (nav != null)
            {
                nav.BarBackgroundColor = Colors.White;
                nav.BarTextColor = Colors.Black;
            }
        }
    }

    public static void HandleAppActions(AppAction appAction)
    {
        if (App.Current is not App app)
        {
            return;
        }

        app.Dispatcher.Dispatch(
            async () =>
            {
                if (appAction.Id.Equals(AppActions.ViewProfileAction.Id))
                {
                    await app._navigationService.NavigateToAsync("//Main/Profile");
                }
            });
    }
}
