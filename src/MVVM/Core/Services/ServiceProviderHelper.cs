//global using Common.Data.Json;
//global using Common.Data.Sql;
global using Common.Data.Api;
using Microsoft.Extensions.DependencyInjection;
using MVVM.Core.Stores;
using MVVM.Core.ViewModels;

namespace MVVM.Core.Services;

/// <summary>Helper class to provide dependency injection services.</summary>
public static class ServiceProviderHelper
{
	/// <summary>Default name of the application settings file.</summary>
	private const string cSettingsFile = "appsettings.json";

	/// <summary>Creates the service provider for the navigation sample.</summary>
	/// <returns>Dependency injection service provider.</returns>
	public static IServiceProvider Create()
	{
		IServiceCollection retValue = new ServiceCollection();

		// Register storage classes
		retValue.AddSingleton<NavigationStore>();
		retValue.AddSingleton<ModalNavigationStore>();
		retValue.AddSingleton<UsersStore>();
		retValue.AddSingleton( CreateSettingsStore );
		retValue.AddSingleton( CreatePeopleStore );
		retValue.AddSingleton( CreateCompaniesStore );

		// Register service classes
		retValue.AddSingleton<CloseModalNavigationService>();
		retValue.AddSingleton( CreateHomeNavigationService );

		// Register view model classes
		retValue.AddSingleton<MainViewModel>();

		retValue.AddTransient( CreateNavigationBarViewModel );
		retValue.AddTransient( CreateHomeViewModel );
		retValue.AddTransient( CreateLoginViewModel );
		retValue.AddTransient( CreateUsersViewModel );
		retValue.AddTransient( CreatePeopleViewModel );
		retValue.AddTransient( CreateCompaniesViewModel );
		retValue.AddTransient( CreateSettingsViewModel );

		// Return the built service provider
		return retValue.BuildServiceProvider();
	}

	#region Create Stores

	private static SettingsStore CreateSettingsStore( IServiceProvider sp )
	{
		_ = new AddressData( configFile: cSettingsFile, useAlpha2: false ); // Address data is required
		return new SettingsStore( cSettingsFile );
	}

	private static PeopleStore CreatePeopleStore( IServiceProvider sp )
	{
		return new PeopleStore( sp.GetRequiredService<SettingsStore>(), new People( cSettingsFile ) );
	}

	private static CompaniesStore CreateCompaniesStore( IServiceProvider sp )
	{
		return new CompaniesStore( sp.GetRequiredService<SettingsStore>(), new Companies( cSettingsFile ) );
	}

	#endregion

	#region Create View Models

	private static HomeViewModel CreateHomeViewModel( IServiceProvider serviceProvider )
	{
		return new HomeViewModel( CreateUsersNavigationService( serviceProvider ) );
	}

	private static LoginViewModel CreateLoginViewModel( IServiceProvider serviceProvider )
	{
		CompositeNavigationService navigationService = new(
			serviceProvider.GetRequiredService<CloseModalNavigationService>(),
			CreateUsersNavigationService( serviceProvider ) );

		return new LoginViewModel( serviceProvider.GetRequiredService<UsersStore>(),
			navigationService );
	}

	private static UsersViewModel CreateUsersViewModel( IServiceProvider serviceProvider )
	{
		return new UsersViewModel( serviceProvider.GetRequiredService<UsersStore>() );
	}

	private static NavigationBarViewModel CreateNavigationBarViewModel( IServiceProvider serviceProvider )
	{
		return new NavigationBarViewModel( serviceProvider.GetRequiredService<UsersStore>(),
			CreateHomeNavigationService( serviceProvider ),
			CreateLoginNavigationService( serviceProvider ),
			CreateUsersNavigationService( serviceProvider ),
			CreatePeopleNavigationService( serviceProvider ),
			CreateCompaniesNavigationService( serviceProvider ),
			CreateSettingsNavigationService( serviceProvider ) );
	}

	private static PeopleViewModel CreatePeopleViewModel( IServiceProvider serviceProvider )
	{
		return new PeopleViewModel( serviceProvider.GetRequiredService<PeopleStore>() );
	}

	private static CompaniesViewModel CreateCompaniesViewModel( IServiceProvider serviceProvider )
	{
		return new CompaniesViewModel( serviceProvider.GetRequiredService<CompaniesStore>() );
	}

	private static SettingsViewModel CreateSettingsViewModel( IServiceProvider serviceProvider )
	{
		CompositeNavigationService navigationService = new(
			serviceProvider.GetRequiredService<CloseModalNavigationService>(),
			CreateHomeNavigationService( serviceProvider ) );

		return new SettingsViewModel(
			serviceProvider.GetRequiredService<SettingsStore>(),
			navigationService );
	}

	#endregion

	#region Create Navigation Services

	private static INavigationService CreateHomeNavigationService( IServiceProvider serviceProvider )
	{
		return new LayoutNavigationService<HomeViewModel>(
			serviceProvider.GetRequiredService<NavigationStore>(),
			() => serviceProvider.GetRequiredService<HomeViewModel>(),
			() => serviceProvider.GetRequiredService<NavigationBarViewModel>() );
	}

	private static INavigationService CreateLoginNavigationService( IServiceProvider serviceProvider )
	{
		return new ModalNavigationService<LoginViewModel>(
			serviceProvider.GetRequiredService<ModalNavigationStore>(),
			() => serviceProvider.GetRequiredService<LoginViewModel>() );
	}

	private static INavigationService CreateUsersNavigationService( IServiceProvider serviceProvider )
	{
		return new LayoutNavigationService<UsersViewModel>(
			serviceProvider.GetRequiredService<NavigationStore>(),
			() => serviceProvider.GetRequiredService<UsersViewModel>(),
			() => serviceProvider.GetRequiredService<NavigationBarViewModel>() );
	}

	private static INavigationService CreatePeopleNavigationService( IServiceProvider serviceProvider )
	{
		return new LayoutNavigationService<PeopleViewModel>(
			serviceProvider.GetRequiredService<NavigationStore>(),
			() => serviceProvider.GetRequiredService<PeopleViewModel>(),
			() => serviceProvider.GetRequiredService<NavigationBarViewModel>() );
	}

	private static INavigationService CreateCompaniesNavigationService( IServiceProvider serviceProvider )
	{
		return new LayoutNavigationService<CompaniesViewModel>(
			serviceProvider.GetRequiredService<NavigationStore>(),
			() => serviceProvider.GetRequiredService<CompaniesViewModel>(),
			() => serviceProvider.GetRequiredService<NavigationBarViewModel>() );
	}

	private static INavigationService CreateSettingsNavigationService( IServiceProvider serviceProvider )
	{
		return new ModalNavigationService<SettingsViewModel>(
			serviceProvider.GetRequiredService<ModalNavigationStore>(),
			() => serviceProvider.GetRequiredService<SettingsViewModel>() );
	}

	#endregion
}