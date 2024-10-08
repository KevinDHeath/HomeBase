namespace Library.Core.Stores;

/// <summary>Navigation store.</summary>
public sealed class NavigationStore
{
	private ViewModelBase? _currentViewModel;

	/// <summary>Gets or sets the current view model.</summary>
	public ViewModelBase? CurrentViewModel
	{
		get => _currentViewModel;
		set
		{
			_currentViewModel?.Dispose();
			_currentViewModel = value;
			OnCurrentViewModelChanged();
		}
	}

	/// <summary>Occurs when the current view model changes.</summary>
	public event Action? CurrentViewModelChanged;

	private void OnCurrentViewModelChanged()
	{
		CurrentViewModelChanged?.Invoke();
	}
}