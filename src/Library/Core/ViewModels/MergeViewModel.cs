namespace Library.Core.ViewModels;

/// <summary>Creates a new instance of the MergeViewModel class.</summary>
/// <param name="store">Library store.</param>
/// <param name="navigation">Sections navigation service.</param>
public partial class MergeViewModel( LibraryStore store, INavigationService navigation ) : ViewModelBase
{
	private readonly LibraryStore _store = store;
	private readonly INavigationService _navigation = navigation;

	/// <summary>Gets the collection of items to merge.</summary>
	public ObservableCollection<MergeItem> MergeItems => _store.Merges;

	/// <summary>Item selection changed command.</summary>
	[RelayCommand]
	private void ActChanged() => ApplyChangesCommand.NotifyCanExecuteChanged();

	/// <summary>Gets the cancel command.</summary>
	public ICommand CancelCommand { get; } = new NavigateCommand( navigation );

	/// <summary>Apply changes command.</summary>
	[RelayCommand( CanExecute = nameof( CanApplyChanges ) )]
	private void ApplyChanges()
	{
		SortedDictionary<string, List<MergeItem>> sections = ConvertToDictionary( "Section" );
		string? itemsPath = _store.Settings.GetItemsPath();
		string? mergePath = _store.Settings.GetMergePath();

		int count = 0;
		foreach( string key in sections.Keys )
		{
			Section? section = Section.GetSectionByName( _store.Sections.ToList(), key );
			foreach( MergeItem item in sections[key] )
			{
				if( item.PerformMerge() )
				{
					item.Item.Category = Categories.New;
					section?.Items.Add( item.Item );
					_store.ItemsCount++;
					_ = _store.Merges.Remove( item );
					count++;
				}
			}

			if( count > 0 )
			{
				Section.SetCategory( section );
				if( section is not null ) { section.Items = [.. section.Items.OrderBy( x => x.Title )]; }
			}
		}

		if( count > 0 )
		{
			// Need to also set the Authors category
			SortedDictionary<string, List<MergeItem>> authors = ConvertToDictionary( "Author" );
			foreach( string key in authors.Keys )
			{
				Section? section = Section.GetSectionByName( _store.Authors.ToList(), key );
				Section.SetCategory( section );
			}

			_ = _store.Save();
		}

		_navigation.Navigate();
	}

	private SortedDictionary<string, List<MergeItem>> ConvertToDictionary( string property )
	{
		SortedDictionary<string, List<MergeItem>> rtn = [];
		foreach( MergeItem item in MergeItems )
		{
			if( !item.Merge ) { continue; }
			List<MergeItem> coll;
			var info = item.Item.GetType().GetProperty( property );
			string? key = info?.GetValue( item.Item )?.ToString();
			if( key is null ) { continue; }

			if( rtn.TryGetValue( key, out List<MergeItem>? value ) ) { coll = value; }
			else
			{
				coll = new List<MergeItem>();
				rtn.Add( key, coll );
			}
			coll.Add( item);
		}
		return rtn;
	}

	private bool CanApplyChanges() => MergeItems.Any( s => s.Merge );
}