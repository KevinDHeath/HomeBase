using CommunityToolkit.Mvvm.Input;
using MVVM.Core.Services;

namespace MVVM.Core.ViewModels;

/// <summary>Home view model.</summary>
public sealed partial class HomeViewModel : ViewModelBase
{
	#region Properties

	/// <summary>Gets the welcome message.</summary>
	public static string WelcomeMessage => "Welcome to my application.";

	#endregion

	#region Commands

	/// <summary>Navigate to the Service View command.</summary>
	[RelayCommand]
	public void NavigateService() => _service.Navigate();

	#endregion

	#region Variables and Constructor

	private readonly INavigationService _service;

	/// <summary>Initializes a new instance of the HomeViewModel class.</summary>
	/// <param name="service">Navigation service.</param>
	public HomeViewModel( INavigationService service )
	{
		_service = service;
	}

	#endregion
}