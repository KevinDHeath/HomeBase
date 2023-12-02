using MVVM.Core.ViewModels;

namespace MVVM.Core.Stores;

/// <summary>Navigation store.</summary>
public class NavigationStore
{
	#region Properties

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

	#endregion

	#region Public Event

	/// <summary>Occurs when the current view model changes.</summary>
	public event Action? CurrentViewModelChanged;

	#endregion

	#region Private Methods

	private void OnCurrentViewModelChanged()
	{
		CurrentViewModelChanged?.Invoke();
	}

	#endregion
}