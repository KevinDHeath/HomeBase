namespace Library.Data.Models;

/// <summary>This class contains the details of a library.</summary>
public class DataStorage
{
	/// <summary>Gets or sets the collection of sections.</summary>
	[XmlElement( ElementName = "Group" )]
	public List<Section> Sections { get; set; } = [];

	#region Public Methods

	/// <summary>Gets the storage interface for a library file.</summary>
	/// <param name="mainFile">Library file name.</param>
	/// <returns>Null is returned if the file type is not supported.</returns>
	public static IStorage? GetStorage( string? mainFile )
	{
		return Path.GetExtension( mainFile ) switch
		{
			StorageXml.cExt => new StorageXml(),
			StorageJson.cExt => new StorageJson(),
			_ => null
		};
	}

	/// <summary>Creates a backups of the data file before serialization.</summary>
	/// <param name="fileName">Full name of the file.</param>
	/// <returns>True if it was successfully backed up, otherwise false is returned.</returns>
	public static bool Backup( string fileName )
	{
		if( string.IsNullOrWhiteSpace( fileName ) ) return false;
		try
		{
			FileInfo fi = new( fileName );
			if( !fi.Exists ) return false;

			string? bak = GetBackupFileName( fi.FullName );
			if( bak is null ) return false;

			// Rename the file
			File.Move( fi.FullName, bak );

			// Remove archive attribute
			FileInfo bakfi = new( bak );
			File.SetAttributes( bakfi.FullName, bakfi.Attributes & ~FileAttributes.Archive );

			return true;
		}
		catch( Exception ex )
		{
			Trace.WriteLine( "DataStorage.Backup() failed!" );
			Trace.WriteLine( ex );
			return false;
		}
	}

	#endregion

	#region Private Methods

	private static string? GetBackupFileName( string? fileName )
	{
		if( string.IsNullOrWhiteSpace( fileName ) ) return null;

		string date = DateTime.Now.ToString( @"yyyyMMdd" );
		try
		{
			FileInfo fileInfo = new( fileName );
			var dir = fileInfo.DirectoryName is not null ? fileInfo.DirectoryName : string.Empty;
			if( string.IsNullOrEmpty( dir ) ) return null;

			string prefix = Path.Combine( dir, date ) + "_";
			string ext = Path.GetExtension( fileInfo.Name );

			string pattern = Path.GetFileNameWithoutExtension( prefix + fileInfo.Name ) + '*' + ext;
			string[] bakFiles = Directory.GetFiles( dir, pattern );

			return 0 == bakFiles.Length
				? prefix + fileInfo.Name
				: prefix + Path.GetFileNameWithoutExtension( fileInfo.Name ) +
					"~" + bakFiles.Length.ToString() + ext;
		}
		catch( Exception ex )
		{
			Trace.WriteLine( "DataStorage.GetBackupFileName() failed!" );
			Trace.WriteLine( ex );
			return null;
		}
	}

	#endregion
}