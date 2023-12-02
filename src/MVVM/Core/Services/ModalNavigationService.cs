using MVVM.Core.Stores;
using MVVM.Core.ViewModels;

namespace MVVM.Core.Services;

/// <summary>Modal navigation service.</summary>
/// <typeparam name="TViewModel">Type of view model.</typeparam>
public class ModalNavigationService<TViewModel> : INavigationService where TViewModel : ViewModelBase
{
	#region Variables and Constructor

	private readonly ModalNavigationStore _navigationStore;
	private readonly Func<TViewModel> _createViewModel;

	/// <summary>Initializes a new instance of the ModalNavigationService class.</summary>
	/// <param name="navigationStore">Modal navigation store.</param>
	/// <param name="createViewModel">Function to create a view model.</param>
	public ModalNavigationService( ModalNavigationStore navigationStore, Func<TViewModel> createViewModel )
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