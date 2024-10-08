namespace Library.Core.Stores;

/// <summary>Library storage.</summary>
public sealed class LibraryStore
{
	private string? _mainFile;
	private IStorage? _storage;
	private DataStorage? _data;
	private readonly SettingsStore _settingsStore;
	private const string cUnknown = "Unknown";

	/// <summary>Initializes a new instance of the LibraryStore class.</summary>
	/// <param name="settingsStore">Settings storage.</param>
	public LibraryStore( SettingsStore settingsStore )
	{
		_settingsStore = settingsStore;
		settingsStore.SettingsChanged += SettingsPropertyChanged;
		Initialize();
	}

	/// <summary>Occurs when the settings property changes.</summary>
	public event Action? SettingsChanged;

	/// <summary>Gets the library title.</summary>
	public string Title { get; private set; } = cUnknown;

	/// <summary>Gets the collection of sections.</summary>
	public ObservableCollection<Section> Sections { get; private set; } = [];

	/// <summary>Gets the collection of authors.</summary>
	public ObservableCollection<Section> Authors { get; private set; } = [];

	/// <summary>Gets the collection of merges.</summary>
	public ObservableCollection<MergeItem> Merges { get; private set; } = [];

	/// <summary>Gets the application settings.</summary>
	public Settings Settings => _settingsStore.Settings;

	/// <summary>Indicates if any Section changes have been made.</summary>
	public ObservableCollection<Section> SectionChanges { get; private set; } = [];

	/// <summary>Indicates if any Item changes have been made.</summary>
	public ObservableCollection<Item> ItemChanges { get; private set; } = [];

	/// <summary>Saves the library file.</summary>
	/// <returns>True if the file was saved.</returns>
	public bool Save()
	{
		if( _storage is null || _data is null || _mainFile is null ) { return false; }

		bool rtn;
		if( Settings.Backup )
		{
			rtn = DataStorage.Backup( _mainFile );
			if( !rtn ) { return rtn; }
		}

		rtn = _storage.Save( _data, _mainFile );
		if( rtn ) { Initialize(); }
		return rtn;
	}

	internal Section? CurrentSection { get; set; }

	internal Item? CurrentItem { get; set; }

	internal int ItemsCount { get; set; }

	internal void ViewItem()
	{
		FileInfo? fi = GetFileInfo();
		if( fi is not null ) { ProcessStart( fi.FullName ); }
	}

	internal void MaintainItem()
	{
		if( !SettingsCheck.IsItemViewerValid( Settings.ItemViewer ) ) { return; }
		ProcessStart( Settings.ItemViewer, GetFileInfo() );
	}

	private void Initialize()
	{
		_mainFile = _settingsStore.Settings.GetMainFileName();
		Title = Settings.GetCollectionName() ?? cUnknown;
		CurrentSection = null; CurrentItem = null;
		Sections.Clear(); Authors.Clear();
		Merges.Clear(); SectionChanges.Clear(); ItemChanges.Clear();

		_storage = DataStorage.GetStorage( Settings.MainFile );
		_data = _storage?.Load( _mainFile );
		_data ??= new DataStorage();
		LoadMerge( _data.Sections );

		ItemsCount = 0;
		SortedDictionary<string, ObservableCollection<Item>> authors = new();
		foreach( Section section in _data.Sections )
		{
			section.Initialize( authors );
			Sections.Add( section );
			ItemsCount += section.Count;
		}

		// Load the authors
		foreach( var kvp in authors )
		{
			Section section = new() { Name = kvp.Key, Items = kvp.Value.ToList(), ItemList = kvp.Value };
			section.Initialize();
			Authors.Add( section );
		}
	}

	private void SettingsPropertyChanged()
	{
		string? mainFile = Settings.GetMainFileName();
		if( Settings.IsValid && !string.IsNullOrWhiteSpace( mainFile ) && _mainFile != mainFile )
		{
			Initialize();
			OnSettingsChanged();
		}
	}

	private void OnSettingsChanged() => SettingsChanged?.Invoke();

	private void LoadMerge( List<Section> sections )
	{
		foreach( Section s in sections )
		{ foreach( Item i in s.Items ) { s.ItemList.Add( i ); } }

		string? itemsPath = Settings.GetItemsPath();
		string? mergePath = Settings.GetMergePath();
		if( string.IsNullOrWhiteSpace( itemsPath ) || string.IsNullOrWhiteSpace( mergePath ) ) { return; }

		DirectoryInfo diNew = new( mergePath );
		foreach( DirectoryInfo di in diNew.GetDirectories() )
		{
			string key = Title + "/" + di.Name;
			Section? section = Section.GetSectionByLocation( sections, key );
			if( section is null ) { continue; }

			foreach( FileInfo fi in di.GetFiles( "*.htm*" ) )
			{
				MergeItem item = new( fi, itemsPath, mergePath, section );
				Merges.Add( item );
				section.ItemList.Add( item.Item );
			}
		}
	}

	private FileInfo? GetFileInfo()
	{
		if( CurrentSection is null || CurrentItem is null ) { return null; }
		Section? section = Section.GetSectionByName( Sections.ToList(), CurrentItem.Section );
		FileInfo? fi = CurrentItem.GetFullPath( Settings.GetItemsPath(), Settings.GetMergePath(), section );
		return fi is null || !fi.Exists ? null : fi;
	}

	private static void ProcessStart( string launch, FileInfo? argument = null )
	{
		ProcessStartInfo startInfo = new( launch ) { UseShellExecute = true };
		if( argument is not null ) { startInfo.Arguments = argument.FullName; }
		Process.Start( startInfo );
	}
}