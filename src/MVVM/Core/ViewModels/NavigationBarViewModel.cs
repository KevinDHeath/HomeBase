using CommunityToolkit.Mvvm.Input;
using MVVM.Core.Services;
using MVVM.Core.Stores;

namespace MVVM.Core.ViewModels;

/// <summary>Navigation bar view model.</summary>
public sealed partial class NavigationBarViewModel : ViewModelBase
{
	#region Properties

	/// <summary>Determines if an account is logged in.</summary>
	public bool IsLoggedIn => _store.IsLoggedIn;

	/// <summary>Determines if an account is logged out.</summary>
	public bool IsLoggedOut => !_store.IsLoggedIn;

	/// <summary>Indicates whether the Hamburger menu is visible.</summary>
	public bool IsMenuOpen
	{
		get => _isMenuOpen;
		set => SetProperty( ref _isMenuOpen, value );
	}

	#endregion

	#region Commands

	/// <summary>Navigate to the Home View command.</summary>
	[RelayCommand]
	public void NavigateHome() => _home.Navigate();

	/// <summary>Navigate to the Login View command.</summary>
	[RelayCommand]
	public void NavigateLogin() => _login.Navigate();

	/// <summary>Navigate to the Users View command.</summary>
	[RelayCommand]
	public void NavigateUsers() => _users.Navigate();

	/// <summary>Navigate to the People View command.</summary>
	[RelayCommand]
	public void NavigatePeople() => _people.Navigate();

	/// <summary>Navigate to the Companies View command.</summary>
	[RelayCommand]
	public void NavigateCompanies() => _companies.Navigate();

	/// <summary>Navigate to the Settings View command.</summary>
	[RelayCommand]
	public void NavigateSettings() => _settings.Navigate();

	/// <summary>Log out command.</summary>
	[RelayCommand]
	public void Logout() => _store.Logout();

	#endregion

	#region Constructor and Variables

	private readonly UsersStore _store;
	private readonly INavigationService _home;
	private readonly INavigationService _settings;
	private readonly INavigationService _login;
	private readonly INavigationService _users;
	private readonly INavigationService _people;
	private readonly INavigationService _companies;
	private bool _isMenuOpen;

	/// <summary>Initializes a new instance of the NavigationBarViewModel class.</summary>
	/// <param name="usersStore">Usages storage.</param>
	/// <param name="homeService">Home navigation service.</param>
	/// <param name="loginService">Login navigation service.</param>
	/// <param name="usersService">Users navigation service.</param>
	/// <param name="peopleService">People navigation service.</param>
	/// <param name="companiesService">Companies navigation service.</param>
	/// <param name="settingsService">Settings navigation service.</param>
	public NavigationBarViewModel( UsersStore usersStore,
		INavigationService homeService,
		INavigationService loginService,
		INavigationService usersService,
		INavigationService peopleService,
		INavigationService companiesService,
		INavigationService settingsService )
	{
		_store = usersStore;
		_home = homeService;
		_login = loginService;
		_users = usersService;
		_people = peopleService;
		_companies = companiesService;
		_settings = settingsService;

		_store.CurrentLoginChanged += OnCurrentLoginChanged;
	}

	#endregion

	#region Overridden Methods

	/// <inheritdoc/>
	public override void Dispose()
	{
		_store.CurrentLoginChanged -= OnCurrentLoginChanged;
		base.Dispose();
	}

	#endregion

	#region Private Methods

	private void OnCurrentLoginChanged()
	{
		OnPropertyChanged( nameof( IsLoggedIn ) );
		OnPropertyChanged( nameof( IsLoggedOut ) );
	}

	#endregion
}