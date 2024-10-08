namespace Library.Data.Models;

/// <summary>This class contains the validations for application settings.</summary>
public abstract class SettingsCheck : ModelBase
{
	internal string _mainFile = string.Empty;
	internal string _rootPath = string.Empty;
	internal string _newPath = string.Empty;
	internal string _itemViewer = string.Empty;
	internal bool _backup;
	internal int _fontSize = 12;

	#region Properties

	/// <summary>Get or set whether the settings are valid or not.</summary>
	[JsonIgnore]
	public bool IsValid { get; set; }

	#endregion

	#region Methods

	/// <summary>Checks if the settings are valid.</summary>
	public void CheckIfValid()
	{
		if( IsValid ) IsValid = false;

		if( !IsRootPathValid( _rootPath ) ) return;
		if( IsMainFileValid( _rootPath, _mainFile ) > 0 ) return;
		if( !IsNewPathValid( _rootPath, _newPath ) ) return;
		if( !IsItemViewerValid( _itemViewer ) ) return;

		IsValid = true;
	}

	/// <summary>Gets the name of the default collection in the library.</summary>
	/// <param name="fullPath">Set to true to get the complete path returned. The default is false.</param>
	/// <returns>The name or full path of the collection if it exists, otherwise null is returned.</returns>
	public string? GetCollectionName( bool fullPath = false )
	{
		var dftName = Path.GetFileNameWithoutExtension( _mainFile );
		if( !fullPath )
		{
			return dftName;
		}
		else if( IsValid )
		{
			var itemsPath = GetItemsPath();
			if( itemsPath is not null && dftName is not null )
			{
				try
				{
					var di = new DirectoryInfo( Path.Combine( itemsPath, dftName ) );
					if( di.Exists ) return di.FullName;
				}
				catch( Exception )
				{ }
			}
		}

		return null;
	}

	/// <summary>Gets the full path and name of the library index file.</summary>
	/// <returns>The full name if it exists, otherwise null is returned.</returns>
	public string? GetMainFileName()
	{
		if( IsValid )
		{
			try
			{
				var fi = new FileInfo( Path.Combine( _rootPath, _mainFile ) );
				if( fi.Exists ) return fi.FullName;
			}
			catch( Exception )
			{ }
		}

		return null;
	}

	/// <summary>Gets the full path for items in the library.</summary>
	/// <returns>The full path to the folder if it exists, otherwise null is returned.</returns>
	public string? GetItemsPath()
	{
		if( IsValid )
		{
			try
			{
				var fi = new FileInfo( Path.Combine( _rootPath, _mainFile ) );
				if( fi.Exists && fi.DirectoryName is not null ) return fi.DirectoryName;
			}
			catch( Exception )
			{ }
		}

		return null;
	}

	/// <summary>Gets the full path for items waiting to be merged.</summary>
	/// <returns>The full path to the folder if it exists, otherwise null is returned.</returns>
	public string? GetMergePath()
	{
		if( IsValid )
		{
			try
			{
				var di = new DirectoryInfo( Path.Combine( _rootPath, _newPath ) );
				if( di.Exists ) return di.FullName;
			}
			catch( Exception )
			{ }
		}

		return null;
	}

	/// <summary>Checks if the MainFile setting is valid.</summary>
	/// <param name="rootPath">Root path.</param>
	/// <param name="mainFile">Main file to be checked.</param>
	/// <returns>Zero if the file exists, otherwise a positive value is returned:<br/>
	/// 1 = Either the root path or main file is empty.<br/>
	/// 2 = The file type is not supported.<br/>
	/// 3 = The main file must be relative to the root path.<br/>
	/// 4 = The main file does not exist.</returns>
	public static int IsMainFileValid( string rootPath, string mainFile )
	{
		// 1=Either the root path or main file is empty
		if( string.IsNullOrWhiteSpace( rootPath ) ||
			string.IsNullOrWhiteSpace( mainFile ) ) return 1;

		// 2=The file type is not supported
		if( !mainFile.EndsWith( StorageJson.cExt ) &&
			!mainFile.EndsWith( StorageXml.cExt ) ) return 2;

		// 3=The main file must be relative to the root path
		if( File.Exists( mainFile ) ) return 3;

		// 4=The main file must exist
		if( !File.Exists( Path.Combine( rootPath, mainFile ) ) ) return 4;

		return 0;
	}

	/// <summary>Checks if the NewPath setting is valid.</summary>
	/// <param name="rootPath">Root path.</param>
	/// <param name="newPath">New path to be checked.</param>
	/// <returns>True if the path exists, otherwise False is returned.</returns>
	public static bool IsNewPathValid( string rootPath, string newPath )
	{
		if( string.IsNullOrWhiteSpace( rootPath ) ||
			string.IsNullOrWhiteSpace( newPath ) ) return false;

		return Path.Exists( Path.Combine( rootPath, newPath ) );
	}

	/// <summary>Checks if the RootPath setting is valid.</summary>
	/// <param name="rootPath">Root path to be checked.</param>
	/// <returns>True if the path exists, otherwise False is returned.</returns>
	public static bool IsRootPathValid( string rootPath )
	{
		if( string.IsNullOrWhiteSpace( rootPath ) ) return false;

		return Path.Exists( rootPath );
	}

	/// <summary>Checks if the ViewItemPgm setting is valid.</summary>
	/// <param name="itemViewer">Full name of the executable to be checked.</param>
	/// <returns>True no program is specified or the program exists, otherwise False is returned.</returns>
	public static bool IsItemViewerValid( string itemViewer )
	{
		// Viewer name can be empty
		if( string.IsNullOrWhiteSpace( itemViewer ) ) return true;

		return Path.Exists( itemViewer );
	}

	#endregion
}