namespace Library.Core.ViewModels;

/// <summary>Creates a new instance of the ItemViewModel class.</summary>
/// <param name="store">Library store.</param>
/// <param name="navigation">Navigation service.</param>
public sealed partial class ItemViewModel( LibraryStore store, INavigationService navigation ) : ViewModelBase
{
	private readonly LibraryStore _store = store;
	private readonly INavigationService _navigation = navigation;
	private readonly Item _mods = store.CurrentItem is not null ? (Item)store.CurrentItem.Clone() : new Item();

	/// <summary>Gets the cancel command.</summary>
	public ICommand CancelCommand { get; } = new NavigateCommand( navigation );

	/// <summary>Apply changes command.</summary>
	[RelayCommand( CanExecute = nameof( CanApplyChanges ) )]
	private void ApplyChanges()
	{
		if( null == _store.CurrentItem ) { return; }
		_ = _store.ItemChanges.Remove( _store.CurrentItem );
		if( _store.CurrentItem.TrackChanges( _mods ) ) { _store.ItemChanges.Add( _store.CurrentItem ); }
		_ = _mods.ApplyChanges( _store.CurrentItem ); // Must do AFTER track changes check
		Section.SetCategory( Section.GetSectionByName( [.. _store.Sections], _store.CurrentItem.Section ) );
		_navigation.Navigate();
	}

	private bool CanApplyChanges() => !HasErrors && _mods.HasChanges( _store.CurrentItem );

	/// <summary>Gets the list of available categories.</summary>
	public List<Categories> Categories => Enumerations.GetCategories( Status ).ToList();

	/// <summary>Gets or sets the author.</summary>
	public string? Author
	{
		get => _mods.Author;
		set
		{
			_mods.Author = value;
			OnPropertyChanged();
			ApplyChangesCommand.NotifyCanExecuteChanged();
		}
	}

	/// <summary>Gets or sets the creation date.</summary>
	public string? Date
	{
		get => _mods.Date;
		set
		{
			_mods.Date = value;
			OnPropertyChanged();
			ApplyChangesCommand.NotifyCanExecuteChanged();
		}
	}

	/// <summary>Gets or sets the filename.</summary>
	[Required( ErrorMessage = "{0} is required." )]
	public string File
	{
		get => _mods.File;
		set
		{
			if( ValidateProperty( value ) ) { _mods.File = value; }
			OnPropertyChanged();
			ApplyChangesCommand.NotifyCanExecuteChanged();
		}
	}

	/// <summary>Gets or sets the title.</summary>
	[Required( ErrorMessage = "{0} is required." )]
	public string Title
	{
		get => _mods.Title;
		set
		{
			if( ValidateProperty( value ) ) { _mods.Title = value; }
			OnPropertyChanged();
			ApplyChangesCommand.NotifyCanExecuteChanged();
		}
	}

	/// <summary>Gets or sets the status as an enumeration.</summary>
	public Categories Status
	{
		get => _mods.Category;
		set
		{
			_mods.Category = value;
			OnPropertyChanged();
			ApplyChangesCommand.NotifyCanExecuteChanged();
		}
	}
}