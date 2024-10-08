namespace Library.Core.Services;

/// <summary>Navigation layout service.</summary>
/// <param name="store">Navigation store.</param>
/// <param name="contentViewModel">Function to create a content view model.</param>
/// <param name="navigationViewModel">Function to create a navigation view model.</param>
/// <typeparam name="TViewModel">View model type.</typeparam>
public sealed class NavigationLayout<TViewModel>( NavigationStore store,
	Func<TViewModel> contentViewModel, Func<NavigationViewModel> navigationViewModel )
	: INavigationService where TViewModel : ViewModelBase
{
	private readonly NavigationStore _store = store;
	private readonly Func<TViewModel> _contentViewModel = contentViewModel;
	private readonly Func<NavigationViewModel> _navigationViewModel = navigationViewModel;

	/// <inheritdoc/>
	public void Navigate()
	{
		_store.CurrentViewModel = new LayoutViewModel( _navigationViewModel(), _contentViewModel() );
	}
}