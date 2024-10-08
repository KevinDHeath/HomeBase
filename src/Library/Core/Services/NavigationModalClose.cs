namespace Library.Core.Services;

/// <summary>Navigation modal close service.</summary>
/// <param name="store">Modal navigation store.</param>
public sealed class NavigationModalClose( NavigationModalStore store ) : INavigationService
{
	private readonly NavigationModalStore _store = store;

	/// <inheritdoc/>
	public void Navigate() => _store.Close();
}