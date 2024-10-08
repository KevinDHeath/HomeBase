using Microsoft.Extensions.DependencyInjection;

namespace Library.Core.Services;

/// <summary>Provides dependency injection services.</summary>
public sealed class RegisterServices
{
	/// <summary>Default settings file name.</summary>
	public const string cAppSettings = "appsettings.json";

	/// <summary>Creates the service collection.</summary>
	/// <param name="appSettings">Application settings file name.</param>
	/// <returns>Dependency injection service provider.</returns>
	public static IServiceCollection Build( string appSettings = cAppSettings )
	{
		IServiceCollection rtn = new ServiceCollection();

		// Register Stores
		_ = rtn.AddSingleton( new SettingsStore( appSettings ) )
		 .AddSingleton( s => new LibraryStore( s.GetRequiredService<SettingsStore>() ) )
		 .AddSingleton<NavigationStore>()
		 .AddSingleton<NavigationModalStore>();

		// Register ViewModels
		_ = rtn.AddSingleton<MainViewModel>()
		 .AddTransient( s => new AboutViewModel( s.GetRequiredService<LibraryStore>(), SettingsNavigation( s ) ) )
		 .AddTransient( LibraryViewModel )
		 .AddTransient( s => new AuthorsViewModel( s.GetRequiredService<LibraryStore>(), ItemNavigation( s ) ) )
		 .AddTransient( s => new MergeViewModel( s.GetRequiredService<LibraryStore>(), SectionsNavigation( s ) ) )
		 .AddTransient( s => new SectionViewModel( s.GetRequiredService<LibraryStore>(), s.GetRequiredService<NavigationModalClose>() ) )
		 .AddTransient( s => new ItemViewModel( s.GetRequiredService<LibraryStore>(), s.GetRequiredService<NavigationModalClose>() ) )
		 .AddTransient( s => new SettingsViewModel( s.GetRequiredService<SettingsStore>(), s.GetRequiredService<NavigationModalClose>() ) )
		 .AddTransient( NavigationViewModel );

		// Register Navigation services
		_ = rtn.AddSingleton( AboutNavigation )
		 .AddSingleton( SectionsNavigation )
		 .AddSingleton<NavigationModalClose>();

		return rtn;
	}

	private static LibraryViewModel LibraryViewModel( IServiceProvider provider )
	{
		return new LibraryViewModel(
			provider.GetRequiredService<LibraryStore>(),
			SectionNavigation( provider ),
			ItemNavigation( provider ) );
	}

	private static NavigationViewModel NavigationViewModel( IServiceProvider sp )
	{
		return new NavigationViewModel(
			sp.GetRequiredService<LibraryStore>(),
			AboutNavigation( sp ),
			SectionsNavigation( sp ),
			AuthorsNavigation( sp ),
			MergeNavigation( sp ),
			SettingsNavigation( sp ) );
	}

	private static NavigationLayout<AboutViewModel> AboutNavigation( IServiceProvider sp )
	{
		return new NavigationLayout<AboutViewModel>(
			sp.GetRequiredService<NavigationStore>(),
			() => sp.GetRequiredService<AboutViewModel>(),
			() => sp.GetRequiredService<NavigationViewModel>() );
	}

	private static NavigationLayout<LibraryViewModel> SectionsNavigation( IServiceProvider sp )
	{
		return new NavigationLayout<LibraryViewModel>(
			sp.GetRequiredService<NavigationStore>(),
			() => sp.GetRequiredService<LibraryViewModel>(),
			() => sp.GetRequiredService<NavigationViewModel>() );
	}

	private static NavigationLayout<AuthorsViewModel> AuthorsNavigation( IServiceProvider sp )
	{
		return new NavigationLayout<AuthorsViewModel>(
			sp.GetRequiredService<NavigationStore>(),
			() => sp.GetRequiredService<AuthorsViewModel>(),
			() => sp.GetRequiredService<NavigationViewModel>() );
	}

	private static NavigationLayout<MergeViewModel> MergeNavigation( IServiceProvider sp )
	{
		return new NavigationLayout<MergeViewModel>(
			sp.GetRequiredService<NavigationStore>(),
			() => sp.GetRequiredService<MergeViewModel>(),
			() => sp.GetRequiredService<NavigationViewModel>() );
	}

	private static NavigationModal<SectionViewModel> SectionNavigation( IServiceProvider sp )
	{
		return new NavigationModal<SectionViewModel>(
			sp.GetRequiredService<NavigationModalStore>(),
			() => sp.GetRequiredService<SectionViewModel>() );
	}

	private static NavigationModal<ItemViewModel> ItemNavigation( IServiceProvider sp )
	{
		return new NavigationModal<ItemViewModel>(
			sp.GetRequiredService<NavigationModalStore>(),
			() => sp.GetRequiredService<ItemViewModel>() );
	}

	private static NavigationModal<SettingsViewModel> SettingsNavigation( IServiceProvider sp )
	{
		return new NavigationModal<SettingsViewModel>(
			sp.GetRequiredService<NavigationModalStore>(),
			() => sp.GetRequiredService<SettingsViewModel>() );
	}
}