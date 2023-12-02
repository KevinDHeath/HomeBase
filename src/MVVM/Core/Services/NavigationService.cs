using MVVM.Core.Stores;
using MVVM.Core.ViewModels;

namespace MVVM.Core.Services;

/// <summary>Navigation service.</summary>
/// <typeparam name="TViewModel">Type of view model.</typeparam>
public class NavigationService<TViewModel> : INavigationService where TViewModel : ViewModelBase
{
	#region Variables and Constructor

	private readonly NavigationStore _navigationStore;
	private readonly Func<TViewModel> _createViewModel;

	/// <summary>Initializes a new instance of the NavigationService class.</summary>
	/// <param name="navigationStore">Navigation store.</param>
	/// <param name="createViewModel">Function to create a view model.</param>
	public NavigationService( NavigationStore navigationStore, Func<TViewModel> createViewModel )
	{
		_navigationStore = navigationStore;
		_createViewModel = createViewModel;
	}

	#endregion

	#region INavigationService Implementation

	/// <inheritdoc/>
	public void Navigate()
	{
		_navigationStore.CurrentViewModel = _createViewModel();
	}

	#endregion
}