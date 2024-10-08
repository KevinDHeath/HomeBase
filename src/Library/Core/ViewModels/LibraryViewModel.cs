namespace Library.Core.ViewModels;

/// <summary>Creates a new instance of the LibraryViewModel class.</summary>
/// <param name="store">Library store.</param>
/// <param name="sectionNav">Navigation for the edit section modal dialog.</param>
/// <param name="itemNav">Navigation for the edit item modal dialog.</param>
public partial class LibraryViewModel( LibraryStore store,
	INavigationService sectionNav, INavigationService itemNav ) : ViewModelBase
{
	private bool _isCurrentSection;
	private string _sectionFilterText = string.Empty;
	private string _itemFilterText = string.Empty;
	internal readonly LibraryStore _store = store;
	private const StringComparison cComp = StringComparison.OrdinalIgnoreCase;

	/// <summary>Gets the edit section command.</summary>
	public ICommand EditSectionCommand { get; } = new NavigateCommand( sectionNav );

	/// <summary>Gets the edit item command.</summary>
	public ICommand EditItemCommand { get; } = new NavigateCommand( itemNav );

	/// <summary>Gets the maintain item command.</summary>
	[RelayCommand( CanExecute = nameof( CanMaintainItem ) )]
	private void MaintainItem() => _store.MaintainItem();

	private bool CanMaintainItem()
	{
		return _store.CurrentItem is not null && SettingsCheck.IsItemViewerValid( _store.Settings.ItemViewer );
	}

	/// <summary>Gets the view item command.</summary>
	[RelayCommand( CanExecute = nameof( CanViewItem ) )]
	private void ViewItem() => _store.ViewItem();

	private bool CanViewItem() => CurrentItem is not null;

	/// <summary>Gets the collection name.</summary>
	public string? CollectionName => _store.Settings.GetCollectionName();

	/// <summary>Gets the list of sections.</summary>
	public ObservableCollection<Section> Sections => _store.Sections;

	/// <summary>Gets or sets the current selected section.</summary>
	public Section? CurrentSection
	{
		get => _store.CurrentSection;
		set
		{
			_store.CurrentSection = value;
			OnPropertyChanged();
			IsCurrentSection = _store.CurrentSection is not null;
		}
	}

	/// <summary>Indicates whether the Current Section has been set.</summary>
	public bool IsCurrentSection
	{
		get => _isCurrentSection;
		set
		{
			_isCurrentSection = value;
			OnPropertyChanged();
		}
	}

	/// <summary>Get or set the section filter text.</summary>
	public string SectionFilterText
	{
		get => _sectionFilterText;
		set
		{
			if( value is not null )
			{
				_sectionFilterText = value;
				OnPropertyChanged();
			}
		}
	}

	/// <summary>Function to performs the section list filtering.</summary>
	public Func<object, bool> SectionListFilter => ( obj ) =>
	{
		if( SectionFilterText.Length == 0 ) { return true; }
		return obj is Section section && section.Name.Contains( SectionFilterText, cComp );
	};

	/// <summary>Get or set the current selected item.</summary>
	public Item? CurrentItem
	{
		get => _store.CurrentItem;
		set
		{
			_store.CurrentItem = value;
			OnPropertyChanged();
		}
	}

	/// <summary>Get or set the item filter text.</summary>
	public string ItemFilterText
	{
		get => _itemFilterText;
		set
		{
			if( value is not null )
			{
				_itemFilterText = value;
				OnPropertyChanged();
			}
		}
	}

	/// <summary>Function to performs the item list filtering.</summary>
	public Func<object, bool> ItemListFilter => ( obj ) =>
	{
		if( ItemFilterText.Length == 0 ) { return true; }
		return obj is Item item && item.Title.Contains( ItemFilterText, cComp );
	};
}