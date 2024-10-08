namespace Library.Data.Models;

/// <summary>Contains the application settings.</summary>
public sealed class Settings : SettingsCheck, ICloneable
{
	#region Properties

	/// <summary>Gets or sets the name of the library file.</summary>
	public string MainFile
	{
		get => _mainFile;
		set
		{
			if( !value.Equals( _mainFile ) )
			{
				_mainFile = value;
				OnPropertyChanged();
			}
		}
	}

	/// <summary>Gets or sets the relative path to new items.</summary>
	public string NewPath
	{
		get => _newPath;
		set => _newPath = value;
	}

	/// <summary>Gets or sets the full path of the library.</summary>
	public string RootPath
	{
		get => _rootPath;
		set
		{
			if( !value.Equals( _rootPath ) )
			{
				_rootPath = value;
				OnPropertyChanged();
			}
		}
	}

	/// <summary>Gets or sets whether to backup the file before saving.</summary>
	public bool Backup
	{
		get => _backup;
		set => _backup = value;
	}

	/// <summary>Gets or sets the program used to view an Item.</summary>
	public string ItemViewer
	{
		get => _itemViewer;
		set => _itemViewer = value;
	}

	/// <summary>Gets or sets the application font size.</summary>
	public int FontSize
	{
		get => _fontSize;
		set => _fontSize = value;
	}

	#endregion

	#region Methods

	/// <summary>Determines whether the settings have changed.</summary>
	/// <param name="source">Setting values to compare.</param>
	/// <returns>True if any there are any changes, otherwise false is returned.</returns>
	public bool HasChanges( Settings source )
	{
		if( FontSize != source.FontSize ) { return true; }
		if( Backup != source.Backup ) { return true; }
		if( MainFile != source.MainFile ) { return true; }
		if( NewPath != source.NewPath ) { return true; }
		if( RootPath != source.RootPath ) { return true; }
		if( ItemViewer != source.ItemViewer ) { return true; }
		return false;
	}

	/// <inheritdoc/>
	public object Clone()
	{
		return MemberwiseClone();
	}

	#endregion
}