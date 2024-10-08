namespace Library.Core.ViewModels;

/// <summary>Settings view model.</summary>
/// <param name="store">Settings storage.</param>
/// <param name="navigation">Navigation service.</param>
public sealed partial class SettingsViewModel( SettingsStore store, INavigationService navigation ) : ViewModelBase
{
	private readonly SettingsStore _store = store;
	private readonly INavigationService _service = navigation;
	private readonly Settings _mods = (Settings)store.Settings.Clone();

	/// <summary>Gets the cancel command.</summary>
	public ICommand CancelCommand { get; } = new NavigateCommand( navigation );

	private bool CanApplyChanges() => !HasErrors && _mods.HasChanges( _store.Settings );

	/// <summary>Apply changes command.</summary>
	[RelayCommand( CanExecute = nameof( CanApplyChanges ) )]
	private void ApplyChanges()
	{
		_store.Settings = _mods;
		if( _store.Save() ) { _service.Navigate(); }
	}

	#region Properties

	/// <summary>Gets or sets the name of the library file.</summary>
	[Required( ErrorMessage = "{0} is required." )]
	public string MainFile
	{
		get => _mods.MainFile;
		set
		{
			if( ValidateProperty( value ) ) { _mods.MainFile = value; }
			OnPropertyChanged();
			ApplyChangesCommand.NotifyCanExecuteChanged();
		}
	}

	/// <summary>Gets or sets the relative path to new items.</summary>
	public string NewPath
	{
		get => _mods.NewPath;
		set
		{
			_mods.NewPath = value;
			OnPropertyChanged();
			ApplyChangesCommand.NotifyCanExecuteChanged();
		}
	}

	/// <summary>Gets or sets the full path of the library.</summary>
	[Required( ErrorMessage = "{0} is required." )]
	public string RootPath
	{
		get => _mods.RootPath;
		set
		{
			if( ValidateProperty( value ) ) { _mods.RootPath = value; }
			OnPropertyChanged();
			ApplyChangesCommand.NotifyCanExecuteChanged();
		}
	}

	/// <summary>Gets or sets whether to backup the file before saving.</summary>
	public bool Backup
	{
		get => _mods.Backup;
		set
		{
			_mods.Backup = value;
			OnPropertyChanged();
			ApplyChangesCommand.NotifyCanExecuteChanged();
		}
	}

	/// <summary>Gets or sets the program used to view an Item.</summary>
	public string ItemViewer
	{
		get => _mods.ItemViewer;
		set
		{
			_mods.ItemViewer = value;
			OnPropertyChanged();
			ApplyChangesCommand.NotifyCanExecuteChanged();
		}
	}

	/// <summary>Gets or sets the application font size.</summary>
	[Required( ErrorMessage = "{0} is required." )]
	[Range( 10, 18, ErrorMessage = "{0} must be between {1} and {2}." )]
	public int FontSize
	{
		get => _mods.FontSize;
		set
		{
			ValidateProperty( value );
			_mods.FontSize = value;
			OnPropertyChanged();
			ApplyChangesCommand.NotifyCanExecuteChanged();
		}
	}

	#endregion
}