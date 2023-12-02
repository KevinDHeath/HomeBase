using MVVM.Core.Stores;
using MVVM.Core.ViewModels;

namespace MVVM.Core.Services;

/// <summary>Layout navigation service.</summary>
/// <typeparam name="TViewModel">Type of view model.</typeparam>
public class LayoutNavigationService<TViewModel> : INavigationService where TViewModel : ViewModelBase
{
	#region Variables and Constructor

	private readonly NavigationStore _navigationStore;
	private readonly Func<TViewModel> _createViewModel;
	private readonly Func<NavigationBarViewModel> _createNavigationBarViewModel;

	/// <summary>Initializes a new instance of the LayoutNavigationService class.</summary>
	/// <param name="navigationStore">Navigation store.</param>
	/// <param name="createViewModel">Function to create a view model.</param>
	/// <param name="createNavigationBarViewModel">Function to create a navigation bar view model.</param>
	public LayoutNavigationService( NavigationStore navigationStore, Func<TViewModel> createViewModel,
		Func<NavigationBarViewModel> createNavigationBarViewModel )
	{
		_navigationStore = navigationStore;
		_createViewModel = createViewModel;
		_createNavigationBarViewModel = createNavigationBarViewModel;
	}

	#endregion

	#region INavigationService Implementation

	/// <inheritdoc/>
	public void Navigate()
	{
		_navigationStore.CurrentViewModel = new LayoutViewModel( _createNavigationBarViewModel(),
			_createViewModel() );
	}

	#endregion
}