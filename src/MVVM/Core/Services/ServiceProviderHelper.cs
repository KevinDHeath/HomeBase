global using Common.Data.SQLite;
//global using Common.Data.Json;
//global using Common.Data.Sql;
//global using Common.Data.Api;

using Microsoft.Extensions.DependencyInjection;
using MVVM.Core.Stores;
using MVVM.Core.ViewModels;

namespace MVVM.Core.Services;

/// <summary>Helper class to provide dependency injection services.</summary>
public static class ServiceProviderHelper
{
	/// <summary>Creates the service provider for the navigation sample.</summary>
	/// <returns>Dependency injection service provider.</returns>
	public static IServiceProvider Create()
	{
		IServiceCollection retValue = new ServiceCollection();

		retValue.AddDbContext<EntityContextBase>( ServiceLifetime.Scoped );

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
		_ = new AddressData( useAlpha2: false ); // Address data is required
		return new SettingsStore( "appsettings.json" );
	}

	private static PeopleStore CreatePeopleStore( IServiceProvider sp )
	{
		EntityContextBase context = sp.GetRequiredService<EntityContextBase>();
		return new PeopleStore( sp.GetRequiredService<SettingsStore>(), context );
	}

	private static CompaniesStore CreateCompaniesStore( IServiceProvider sp )
	{
		EntityContextBase context = sp.GetRequiredService<EntityContextBase>();
		return new CompaniesStore( sp.GetRequiredService<SettingsStore>(), context );
	}

	#endregion

	#region Create View Models

	private static HomeViewModel CreateHomeViewModel( IServiceProvider sp )
	{
		return new HomeViewModel( CreateUsersNavigationService( sp ) );
	}

	private static LoginViewModel CreateLoginViewModel( IServiceProvider sp )
	{
		CompositeNavigationService navigationService = new(
			sp.GetRequiredService<CloseModalNavigationService>(),
			CreateUsersNavigationService( sp ) );

		return new LoginViewModel( sp.GetRequiredService<UsersStore>(),
			navigationService );
	}

	private static UsersViewModel CreateUsersViewModel( IServiceProvider sp )
	{
		return new UsersViewModel( sp.GetRequiredService<UsersStore>() );
	}

	private static NavigationBarViewModel CreateNavigationBarViewModel( IServiceProvider sp )
	{
		return new NavigationBarViewModel( sp.GetRequiredService<UsersStore>(),
			CreateHomeNavigationService( sp ),
			CreateLoginNavigationService( sp ),
			CreateUsersNavigationService( sp ),
			CreatePeopleNavigationService( sp ),
			CreateCompaniesNavigationService( sp ),
			CreateSettingsNavigationService( sp ) );
	}

	private static PeopleViewModel CreatePeopleViewModel( IServiceProvider sp )
	{
		return new PeopleViewModel( sp.GetRequiredService<PeopleStore>() );
	}

	private static CompaniesViewModel CreateCompaniesViewModel( IServiceProvider sp )
	{
		return new CompaniesViewModel( sp.GetRequiredService<CompaniesStore>() );
	}

	private static SettingsViewModel CreateSettingsViewModel( IServiceProvider sp )
	{
		CompositeNavigationService navigationService = new(
			sp.GetRequiredService<CloseModalNavigationService>(),
			CreateHomeNavigationService( sp ) );

		return new SettingsViewModel(
			sp.GetRequiredService<SettingsStore>(),
			navigationService );
	}

	#endregion

	#region Create Navigation Services

	private static INavigationService CreateHomeNavigationService( IServiceProvider sp )
	{
		return new LayoutNavigationService<HomeViewModel>(
			sp.GetRequiredService<NavigationStore>(),
			() => sp.GetRequiredService<HomeViewModel>(),
			() => sp.GetRequiredService<NavigationBarViewModel>() );
	}

	private static ModalNavigationService<LoginViewModel> CreateLoginNavigationService( IServiceProvider sp )
	{
		return new ModalNavigationService<LoginViewModel>(
			sp.GetRequiredService<ModalNavigationStore>(),
			() => sp.GetRequiredService<LoginViewModel>() );
	}

	private static LayoutNavigationService<UsersViewModel> CreateUsersNavigationService( IServiceProvider sp )
	{
		return new LayoutNavigationService<UsersViewModel>(
			sp.GetRequiredService<NavigationStore>(),
			() => sp.GetRequiredService<UsersViewModel>(),
			() => sp.GetRequiredService<NavigationBarViewModel>() );
	}

	private static LayoutNavigationService<PeopleViewModel> CreatePeopleNavigationService( IServiceProvider sp )
	{
		return new LayoutNavigationService<PeopleViewModel>(
			sp.GetRequiredService<NavigationStore>(),
			() => sp.GetRequiredService<PeopleViewModel>(),
			() => sp.GetRequiredService<NavigationBarViewModel>() );
	}

	private static LayoutNavigationService<CompaniesViewModel> CreateCompaniesNavigationService( IServiceProvider sp )
	{
		return new LayoutNavigationService<CompaniesViewModel>(
			sp.GetRequiredService<NavigationStore>(),
			() => sp.GetRequiredService<CompaniesViewModel>(),
			() => sp.GetRequiredService<NavigationBarViewModel>() );
	}

	private static ModalNavigationService<SettingsViewModel> CreateSettingsNavigationService( IServiceProvider sp )
	{
		return new ModalNavigationService<SettingsViewModel>(
			sp.GetRequiredService<ModalNavigationStore>(),
			() => sp.GetRequiredService<SettingsViewModel>() );
	}

	#endregion
}