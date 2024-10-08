namespace Library.Core.ViewModels;

/// <summary>Main view model.</summary>
public sealed class MainViewModel : ViewModelBase
{
	private readonly NavigationStore _store;
	private readonly NavigationModalStore _modalStore;

	/// <summary>Gets the current view model.</summary>
	public ViewModelBase? CurrentViewModel => _store.CurrentViewModel;

	/// <summary>Gets the current modal view model.</summary>
	public ViewModelBase? CurrentModalViewModel => _modalStore.CurrentViewModel;

	/// <summary>Indicates whether a modal view is currently open.</summary>
	public bool IsOpen => _modalStore.IsOpen;

	/// <summary>Creates a new instance of the MainViewModel class.</summary>
	/// <param name="store">Navigation store.</param>
	/// <param name="modalStore">Model navigation store.</param>
	public MainViewModel( NavigationStore store, NavigationModalStore modalStore )
	{
		_store = store;
		_modalStore = modalStore;

		_store.CurrentViewModelChanged += OnCurrentViewModelChanged;
		_modalStore.CurrentViewModelChanged += OnCurrentModalViewModelChanged;
	}

	private void OnCurrentViewModelChanged()
	{
		OnPropertyChanged( nameof( CurrentViewModel ) );
	}

	private void OnCurrentModalViewModelChanged()
	{
		OnPropertyChanged( nameof( CurrentModalViewModel ) );
		OnPropertyChanged( nameof( IsOpen ) );
	}
}