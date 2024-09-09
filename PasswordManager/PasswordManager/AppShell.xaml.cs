using PasswordManager.Services;
using PasswordManager.Views;

namespace PasswordManager;

public partial class AppShell : Shell
{
	private readonly INavigationService _navigationService;
	public AppShell(INavigationService navigationService)
	{
		_navigationService = navigationService;

        AppShell.InitializeRouting();
        InitializeComponent();

    }
	
	protected override async void OnHandlerChanged()
	{
		base.OnHandlerChanged();

		if (Handler is not null)
		{
			await _navigationService.InitializeAsync();
		}
	}

	private static void InitializeRouting()
	{
        Routing.RegisterRoute(nameof(AddPasswordPage), typeof(AddPasswordPage));
		//Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
		//Routing.RegisterRoute(nameof(PasswordsPage), typeof(PasswordsPage));
		Routing.RegisterRoute(nameof(EditPasswordPage), typeof(EditPasswordPage));
	}
}
