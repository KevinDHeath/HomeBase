namespace Library.Core.Services;

/// <summary>Navigation modal service.</summary>
/// <param name="navigationStore">Modal navigation store.</param>
/// <param name="createViewModel">Function to create a view model.</param>
/// <typeparam name="TViewModel">View model type.</typeparam>
public sealed class NavigationModal<TViewModel>( NavigationModalStore navigationStore,
	Func<TViewModel> createViewModel ) : INavigationService where TViewModel : ViewModelBase
{
	private readonly NavigationModalStore _navigationStore = navigationStore;
	private readonly Func<TViewModel> _createViewModel = createViewModel;

	/// <inheritdoc/>
	public void Navigate() => _navigationStore.CurrentViewModel = _createViewModel();
}