namespace Library.Core.ViewModels;

/// <summary>Creates a new instance of the SectionViewModel class.</summary>
/// <param name="store">Library store.</param>
/// <param name="navigation">Navigation service.</param>
public sealed partial class SectionViewModel( LibraryStore store, INavigationService navigation ) : ViewModelBase
{
	private readonly LibraryStore _store = store;
	private readonly INavigationService _navigation = navigation;
	private readonly Section _mods = store.CurrentSection is not null ? (Section)store.CurrentSection.Clone() : new Section();

	/// <summary>Gets the cancel command.</summary>
	public ICommand CancelCommand { get; } = new NavigateCommand( navigation );

	/// <summary>Apply changes command.</summary>
	[RelayCommand( CanExecute = nameof( CanApplyChanges ) )]
	private void ApplyChanges()
	{
		if( null == _store.CurrentSection ) { return; }
		_ = _store.SectionChanges.Remove( _store.CurrentSection );
		if( _store.CurrentSection.TrackChanges( _mods ) ) { _store.SectionChanges.Add( _store.CurrentSection ); }
		_ = _mods.ApplyChanges( _store.CurrentSection ); // Must do AFTER track changes check
		// IF Name CHANGED UPDATE ALL ITEMS IN SECTION
		_navigation.Navigate();
	}

	private bool CanApplyChanges() => !HasErrors && _mods.HasChanges( _store.CurrentSection );

	/// <summary>Gets or sets the location.</summary>
	[Required( ErrorMessage = "{0} is required." )]
	public string Location
	{
		get => _mods.Location;
		set
		{
			if( ValidateProperty( value ) ) { _mods.Location = value; }
			OnPropertyChanged();
			ApplyChangesCommand.NotifyCanExecuteChanged();
		}
	}

	/// <summary>Gets or sets the name.</summary>
	[Required( ErrorMessage = "{0} is required." )]
	public string Name
	{
		get => _mods.Name;
		set
		{
			if( ValidateProperty( value ) ) { _mods.Name = value; }
			OnPropertyChanged();
			ApplyChangesCommand.NotifyCanExecuteChanged();
		}
	}
}