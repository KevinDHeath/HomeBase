using MVVM.Core.ViewModels;

namespace MVVM.Core.Stores;

/// <summary>Modal navigation store.</summary>
public class ModalNavigationStore
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

	/// <summary>Determines if the modal view is open.</summary>
	public bool IsOpen => CurrentViewModel is not null;

	#endregion

	#region Public Events and Methods

	/// <summary>Occurs when the current view model changes.</summary>
	public event Action? CurrentViewModelChanged;

	/// <summary>Close the current modal view.</summary>
	public void Close()
	{
		CurrentViewModel = null;
	}

	#endregion

	#region Private Methods

	private void OnCurrentViewModelChanged()
	{
		CurrentViewModelChanged?.Invoke();
	}

	#endregion
}