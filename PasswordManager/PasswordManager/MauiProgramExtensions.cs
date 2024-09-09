using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using PasswordManager.Services;
using PasswordManager.Services.AppEnvironment;
using PasswordManager.Services.RequestProvider;
using PasswordManager.Services.SecureStorages;
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
                fonts.AddFont("Font_Awesome_5_Free-Regular-400.otf", "FontAwesome-Regular");
                fonts.AddFont("Font_Awesome_5_Free-Solid-900.otf", "FontAwesome-Solid");
                fonts.AddFont("Montserrat-Bold.ttf", "Montserrat-Bold");
                fonts.AddFont("Montserrat-Regular.ttf", "Montserrat-Regular");
                fonts.AddFont("SourceSansPro-Regular.ttf", "SourceSansPro-Regular");
                fonts.AddFont("SourceSansPro-Solid.ttf", "SourceSansPro-Solid");
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
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "passwordmanager.db3");
        mauiAppBuilder.Services.AddSingleton(new LocalStorageService(dbPath));
        mauiAppBuilder.Services.AddSingleton<ISettingsService, SettingsService>();
        mauiAppBuilder.Services.AddSingleton<INavigationService, MauiNavigationService>();

        mauiAppBuilder.Services.AddSingleton<IRequestProvider, RequestProvider>();
        mauiAppBuilder.Services.AddSingleton<ICredentialsService, CredentialsService>();
        mauiAppBuilder.Services.AddSingleton<ISecureStorageService, SecureStorageService>();
        
        mauiAppBuilder.Services.AddSingleton<ITheme, Theme>();

        mauiAppBuilder.Services.AddSingleton<IAppEnvironmentService, AppEnvironmentService>(
            serviceProvider =>
            {
                var requestProvider = serviceProvider.GetService<IRequestProvider>();
                var settingsService = serviceProvider.GetService<ISettingsService>();
                var localStorageService = serviceProvider.GetService<LocalStorageService>();

                var aes =
                    new AppEnvironmentService(
                        new CredentialMockService(), new CredentialsService(requestProvider, localStorageService, settingsService));

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
        mauiAppBuilder.Services.AddTransient<EditPasswordViewModel>();

        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransient<PasswordsPage>();
        mauiAppBuilder.Services.AddTransient<AddPasswordPage>();
        mauiAppBuilder.Services.AddTransient<SettingsPage>();
        mauiAppBuilder.Services.AddTransient<EditPasswordPage>();

        return mauiAppBuilder;
    }
}
