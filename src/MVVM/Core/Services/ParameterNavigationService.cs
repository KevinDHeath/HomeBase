using MVVM.Core.Stores;
using MVVM.Core.ViewModels;

namespace MVVM.Core.Services;

/// <summary>Navigation service with a view model that has a parameter.</summary>
/// <typeparam name="TParameter">Type of parameter.</typeparam>
/// <typeparam name="TViewModel">Type of view model.</typeparam>
public class ParameterNavigationService<TParameter, TViewModel> where TViewModel : ViewModelBase
{
	#region Variables and Constructor

	private readonly NavigationStore _navigationStore;
	private readonly Func<TParameter, TViewModel> _createViewModel;

	/// <summary>Initializes a new instance of the ParameterNavigationService class.</summary>
	/// <param name="navigationStore">Navigation store.</param>
	/// <param name="createViewModel">Function to create a view model with a parameter.</param>
	public ParameterNavigationService( NavigationStore navigationStore, Func<TParameter,
		TViewModel> createViewModel )
	{
		_navigationStore = navigationStore;
		_createViewModel = createViewModel;
	}

	#endregion

	#region Public Methods

	/// <summary>Navigate method with a parameter.</summary>
	/// <param name="parameter">View model parameter.</param>
	public void Navigate( TParameter parameter )
	{
		_navigationStore.CurrentViewModel = _createViewModel( parameter );
	}

	#endregion
}