namespace Library.Core.ViewModels;

/// <summary>Creates a new instance of the LayoutViewModel class.</summary>
/// <param name="navViewModel">Navigation view model.</param>
/// <param name="viewModel">Content view model.</param>
public sealed class LayoutViewModel( NavigationViewModel navViewModel, ViewModelBase viewModel ) : ViewModelBase
{
	private readonly NavigationViewModel _navigationViewModel = navViewModel;
	private readonly ViewModelBase _contentViewModel = viewModel;

	/// <summary>Gets the Navigation view model.</summary>
	public NavigationViewModel NavigationViewModel  => _navigationViewModel;

	/// <summary>Gets the Content view model.</summary>
	public ViewModelBase ContentViewModel => _contentViewModel;

	/// <inheritdoc/>
	public override void Dispose()
	{
		NavigationViewModel.Dispose();
		ContentViewModel.Dispose();
		base.Dispose();
	}
}