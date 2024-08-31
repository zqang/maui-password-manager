using PasswordManager.Services;
using PasswordManager.Views;

namespace PasswordManager;

public partial class AppShell : Shell
{
	private readonly INavigationService _navigationService;
	public AppShell(INavigationService navigationService)
	{
		_navigationService = navigationService;
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
	}
}
