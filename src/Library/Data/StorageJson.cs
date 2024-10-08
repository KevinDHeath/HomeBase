global using System.Text.Json.Serialization;
using System.Text.Json;

namespace Library.Data;

/// <summary>This class uses JSON data for a library.</summary>
public sealed class StorageJson : IStorage
{
	/// <summary>Initializes a new instance of the JsonStorage class.</summary>
	[EditorBrowsable( EditorBrowsableState.Never )]
	public StorageJson() { }

	/// <summary>JSON file extension.</summary>
	public const string cExt = ".json";

	#region IStorage Implementation

	/// <summary>Loads data from a JSON file.</summary>
	/// <inheritdoc/>
	public DataStorage Load( string? fileName = null )
	{
		DataStorage rtn = new();
		if( string.IsNullOrWhiteSpace( fileName ) ) { return rtn; }
		try
		{
			FileInfo fi = new( fileName );
			return !fi.Exists ? rtn : DeserializeJson( fi.FullName );
		}
		catch( Exception ex )
		{
			Trace.WriteLine( "JsonStorage.Load() failed!" );
			Trace.WriteLine( ex );
			return rtn;
		}
	}

	/// <summary>Saves data to a JSON file.</summary>
	/// <inheritdoc/>
	public bool Save( DataStorage data, string? fileName )
	{
		if( string.IsNullOrWhiteSpace( fileName ) ) return false;
		try
		{
			string json = JsonSerializer.Serialize( data, GetOptions( false ) );
			if( !string.IsNullOrWhiteSpace( json ) )
			{
				File.WriteAllText( fileName, json );
				return true;
			}
		}
		catch( Exception ex )
		{
			Trace.WriteLine( "JsonStorage.Save() failed!" );
			Trace.WriteLine( ex );
		}
		return false;
	}

	#endregion

	#region Private Methods

	private static DataStorage DeserializeJson( string fileName )
	{
		DataStorage? obj = JsonSerializer.Deserialize<DataStorage>(
			File.ReadAllText( fileName, System.Text.Encoding.ASCII ), GetOptions() );

		return obj is not null ? obj : new DataStorage();
	}

	private static JsonSerializerOptions GetOptions( bool indent = false ) => new()
	{
		Converters = { new JsonStringEnumConverter(), },
		Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
		IgnoreReadOnlyProperties = true,
		PropertyNameCaseInsensitive = true,
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
		WriteIndented = indent,
	};

	#endregion
}