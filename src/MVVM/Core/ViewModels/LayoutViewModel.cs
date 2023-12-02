namespace MVVM.Core.ViewModels;

/// <summary>Layout view model.</summary>
public sealed class LayoutViewModel : ViewModelBase
{
	#region Properties

	/// <summary>Gets the navigation bar view model.</summary>
	public NavigationBarViewModel NavigationBarViewModel { get; }

	/// <summary>Gets the content view model.</summary>
	public ViewModelBase ContentViewModel { get; }

	#endregion

	#region Constructor

	/// <summary>Initializes a new instance of the LayoutViewModel class.</summary>
	/// <param name="navigationBarViewModel">Navigation bar view model.</param>
	/// <param name="contentViewModel">Content view model.</param>
	public LayoutViewModel( NavigationBarViewModel navigationBarViewModel, ViewModelBase contentViewModel )
	{
		NavigationBarViewModel = navigationBarViewModel;
		ContentViewModel = contentViewModel;
	}

	#endregion

	#region Overridden Methods

	/// <inheritdoc/>
	public override void Dispose()
	{
		NavigationBarViewModel.Dispose();
		ContentViewModel.Dispose();
		base.Dispose();
	}

	#endregion
}