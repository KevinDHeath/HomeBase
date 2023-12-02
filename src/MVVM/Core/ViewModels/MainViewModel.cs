using MVVM.Core.Stores;

namespace MVVM.Core.ViewModels;

/// <summary>Main view model.</summary>
public sealed class MainViewModel : ViewModelBase
{
	#region Properties

	/// <summary>Gets the current view model.</summary>
	public ViewModelBase? CurrentViewModel => _navigationStore.CurrentViewModel;

	/// <summary>Gets the current modal view model.</summary>
	public ViewModelBase? CurrentModalViewModel => _modalNavigationStore.CurrentViewModel;

	/// <summary>Determines if a modal view is currently open.</summary>
	public bool IsOpen => _modalNavigationStore.IsOpen;

	#endregion

	#region Variables and Constructor

	private readonly NavigationStore _navigationStore;
	private readonly ModalNavigationStore _modalNavigationStore;

	/// <summary>Initializes a new instance of the LoginViewModel class.</summary>
	/// <param name="navigationStore">Navigation store.</param>
	/// <param name="modalNavigationStore">Modal navigation store.</param>
	public MainViewModel( NavigationStore navigationStore, ModalNavigationStore modalNavigationStore )
	{
		_navigationStore = navigationStore;
		_modalNavigationStore = modalNavigationStore;

		_navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
		_modalNavigationStore.CurrentViewModelChanged += OnCurrentModalViewModelChanged;
	}

	#endregion

	#region Private Methods

	private void OnCurrentViewModelChanged()
	{
		OnPropertyChanged( nameof( CurrentViewModel ) );
	}

	private void OnCurrentModalViewModelChanged()
	{
		OnPropertyChanged( nameof( CurrentModalViewModel ) );
		OnPropertyChanged( nameof( IsOpen ) );
	}

	#endregion
}