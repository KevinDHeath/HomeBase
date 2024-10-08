namespace Library.Core.Commands;

/// <summary>A command whose sole purpose is to navigate to another service.</summary>
/// <param name="navigationService">Navigation service.</param>
public class NavigateCommand( INavigationService navigationService ) : CommandBase
{
	private readonly INavigationService _navigationService = navigationService;

	/// <inheritdoc/>
	public override void Execute( object? parameter )
	{
		_navigationService.Navigate();
	}
}