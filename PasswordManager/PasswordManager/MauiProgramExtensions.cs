using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using PasswordManager.Services;
using PasswordManager.Services.AppEnvironment;
using PasswordManager.Services.RequestProvider;
using PasswordManager.Services.Settings;
using PasswordManager.Services.Theme;
using PasswordManager.Views;

namespace PasswordManager;

public static class MauiProgramExtensions
{
	public static MauiAppBuilder UseSharedMauiApp(this MauiAppBuilder builder)
	{
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.RegisterAppServices()
			.RegisterViewModels()
			.RegisterViews();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder;
	}
	public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<ISettingsService, SettingsService>();
        mauiAppBuilder.Services.AddSingleton<INavigationService, MauiNavigationService>();

        mauiAppBuilder.Services.AddSingleton<IRequestProvider, RequestProvider>();
        mauiAppBuilder.Services.AddSingleton<ICredentialsService, CredentialsService>();
        
        mauiAppBuilder.Services.AddSingleton<ITheme, Theme>();

        mauiAppBuilder.Services.AddSingleton<IAppEnvironmentService, AppEnvironmentService>(
            serviceProvider =>
            {
                var requestProvider = serviceProvider.GetService<IRequestProvider>();
                var settingsService = serviceProvider.GetService<ISettingsService>();

                var aes =
                    new AppEnvironmentService(
                        new CredentialMockService(), new CredentialsService(requestProvider));

                aes.UpdateDependencies(settingsService.UseMocks);
                return aes;
            });

#if DEBUG
        mauiAppBuilder.Logging.AddDebug();
#endif

        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransient<PasswordsViewModel>();
        mauiAppBuilder.Services.AddTransient<SettingsViewModel>();
        mauiAppBuilder.Services.AddTransient<AddPasswordViewModel>();

        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransient<PasswordsPage>();
        mauiAppBuilder.Services.AddTransient<AddPasswordPage>();
        mauiAppBuilder.Services.AddTransient<SettingsPage>();

        return mauiAppBuilder;
    }
}
