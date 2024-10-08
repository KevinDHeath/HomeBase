namespace Library.Core.Services;

/// <summary>Navigate multiple service.</summary>
/// <param name="services">Collection of navigation services.</param>
public sealed class NavigationMulti( params INavigationService[] services ) : INavigationService
{
	private readonly IEnumerable<INavigationService> _navigationServices = services;

	/// <inheritdoc/>
	public void Navigate()
	{
		foreach( INavigationService navigationService in _navigationServices )
		{
			navigationService.Navigate();
		}
	}
}