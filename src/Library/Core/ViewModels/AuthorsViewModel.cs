namespace Library.Core.ViewModels;

/// <summary>Creates a new instance of the AuthorsViewModel class.</summary>
/// <param name="store">Library store.</param>
/// <param name="navigation">Navigation for the Library view model.</param>
public sealed partial class AuthorsViewModel( LibraryStore store, INavigationService navigation )
	: LibraryViewModel( store, navigation, navigation )
{
	/// <summary>Gets the list of authors.</summary>
	public ObservableCollection<Section> Authors => _store.Authors;
}