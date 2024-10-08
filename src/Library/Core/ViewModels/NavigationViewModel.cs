using System.Collections.Specialized;

namespace Library.Core.ViewModels;

/// <summary>Navigation view model</summary>
public partial class NavigationViewModel : ViewModelBase
{
	private bool _isMenuOpen;
	private string _title = string.Empty;
	private readonly LibraryStore _store;

	/// <summary>Creates a new instance of the NavigationViewModel class.</summary>
	/// <param name="store">Library store.</param>
	/// <param name="aboutNavigation">Navigation for the About view.</param>
	/// <param name="sectionsNavigation">Navigation for the Sections view.</param>
	/// <param name="authorsNavigation">Navigation for the Authors view.</param>
	/// <param name="mergeNavigation">Navigation for the Merge view.</param>
	/// <param name="settingsNavigation">Navigation for the Settings view.</param>
	public NavigationViewModel( LibraryStore store, INavigationService aboutNavigation,
		INavigationService sectionsNavigation, INavigationService authorsNavigation,
		INavigationService mergeNavigation, INavigationService settingsNavigation )
	{
		_store = store;
		_store.SettingsChanged += SettingsPropertyChanged;
		SettingsPropertyChanged();

		_store.SectionChanges.CollectionChanged += SectionChanged;
		_store.ItemChanges.CollectionChanged += ItemChanged;
		_store.Merges.CollectionChanged += MergeChanged;
		NavigateAboutCommand = new NavigateCommand( aboutNavigation );
		NavigateSectionsCommand = new NavigateCommand( sectionsNavigation );
		NavigateAuthorsCommand = new NavigateCommand( authorsNavigation );
		NavigateMergeCommand = new NavigateCommand( mergeNavigation );
		NavigateSettingsCommand = new NavigateCommand( settingsNavigation );
	}

	/// <summary>Gets the menu title.</summary>
	public string MenuTitle
	{
		get => _title;
		set
		{
			_title = value;
			OnPropertyChanged();
		}
	}

	/// <summary>Gets whether there are Merges.</summary>
	public bool HasMerges => _store.Merges.Count > 0;

	/// <summary>Gets whether there are Item changes to save.</summary>
	public bool HasChanges => _store.SectionChanges.Count > 0 || _store.ItemChanges.Count > 0;

	/// <summary>Gets the navigation command for the About view.</summary>
	public ICommand NavigateAboutCommand { get; }

	/// <summary>Gets the navigation command for the Section view.</summary>
	public ICommand NavigateSectionsCommand { get; }

	/// <summary>Gets the navigation command for the Section view.</summary>
	public ICommand NavigateAuthorsCommand { get; }

	/// <summary>Gets the navigation command for the Merge view.</summary>
	public ICommand NavigateMergeCommand { get; }

	/// <summary>Gets the navigation command for the Settings view.</summary>
	public ICommand NavigateSettingsCommand { get; }

	/// <summary>Apply changes command.</summary>
	[RelayCommand()]
	private void SaveChanges()
	{
		_store.Save();
		NavigateAboutCommand.Execute( null );
	}

	/// <summary>Gets or sets whether the menu is open.</summary>
	public bool IsMenuOpen
	{
		get => _isMenuOpen;
		set
		{
			if( value.Equals( _isMenuOpen ) ) return;
			_isMenuOpen = value;
			OnPropertyChanged();
		}
	}

	private void MergeChanged( object? sender, NotifyCollectionChangedEventArgs e ) => OnPropertyChanged( nameof( HasMerges ) );

	private void SectionChanged( object? sender, NotifyCollectionChangedEventArgs e ) => OnPropertyChanged( nameof( HasChanges ) );

	private void ItemChanged( object? sender, NotifyCollectionChangedEventArgs e ) => OnPropertyChanged( nameof( HasChanges ) );

	private void SettingsPropertyChanged()
	{
		string? mainFile = _store.Settings.GetMainFileName();
		if( _store.Settings.IsValid && !string.IsNullOrWhiteSpace( mainFile ) )
		{
			MenuTitle = _store.Title;
		}
	}

	/// <inheritdoc/>
	public override void Dispose()
	{
		_store.SettingsChanged -= SettingsPropertyChanged;
		_store.Merges.CollectionChanged -= MergeChanged;
		_store.SectionChanges.CollectionChanged -= SectionChanged;
		_store.ItemChanges.CollectionChanged -= ItemChanged;
		base.Dispose();
	}
}