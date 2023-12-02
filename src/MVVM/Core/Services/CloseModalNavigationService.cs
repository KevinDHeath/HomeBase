using MVVM.Core.Stores;

namespace MVVM.Core.Services;

/// <summary>Close modal navigation service.</summary>
public class CloseModalNavigationService : INavigationService
{
	#region Variables and Constructor

	private readonly ModalNavigationStore _navigationStore;

	/// <summary>Initializes a new instance of the CloseModalNavigationService class.</summary>
	/// <param name="store">Modal navigation store.</param>
	public CloseModalNavigationService( ModalNavigationStore store )
	{
		_navigationStore = store;
	}

	#endregion

	#region INavigationService Implementation

	/// <inheritdoc/>
	public void Navigate()
	{
		_navigationStore.Close();
	}

	#endregion
}