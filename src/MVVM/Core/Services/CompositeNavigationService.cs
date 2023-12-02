namespace MVVM.Core.Services;

/// <summary>Composite navigation service.</summary>
public class CompositeNavigationService : INavigationService
{
	#region Variables and Constructor

	private readonly IEnumerable<INavigationService> _navigationServices;

	/// <summary>Initializes a new instance of the CompositeNavigationService class.</summary>
	/// <param name="services">Collection of navigation services.</param>
	public CompositeNavigationService( params INavigationService[] services )
	{
		_navigationServices = services;
	}

	#endregion

	#region INavigationService Implementation

	/// <inheritdoc/>
	public void Navigate()
	{
		foreach( INavigationService navigationService in _navigationServices )
		{
			navigationService.Navigate();
		}
	}

	#endregion
}