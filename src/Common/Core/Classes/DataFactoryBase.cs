using System.Text;
using System.Text.Json;

namespace Common.Core.Classes;

/// <summary>Base class for Data Factory classes.</summary>
public abstract class DataFactoryBase
{
	/// <summary>Default application settings file name.</summary>
	public const string cConfigFile = "appsettings.json";

	/// <summary>Represents a pseudo-random number generator.</summary>
	protected static readonly Random sRandom = new();

	/// <summary>Serializes the Json data to a disk file.</summary>
	/// <param name="obj">Object containing the data.</param>
	/// <param name="path">Location for the Json data file.</param>
	/// <param name="file">Name of the file. If not supplied the default name is used.</param>
	/// <param name="options">Serialization options.</param>
	/// <returns>True if the data was serialized, otherwise false is returned.</returns>
	protected static bool SerializeJson( object obj, string path, string file, JsonSerializerOptions options )
	{
		return JsonHelper.Serialize( obj, Path.Combine( path, file ), options );
	}

	/// <summary>Reads a Json disk file and populates a factory object.</summary>
	/// <typeparam name="T">Type of factory to populate.</typeparam>
	/// <param name="path">Location of the data file.</param>
	/// <param name="file">Name of the file.</param>
	/// <param name="options">Json serializer options.</param>
	/// <returns>Null is returned if the object could not be populated.</returns>
	protected static T? DeserializeJson<T>( string path, string file, JsonSerializerOptions options ) where T : DataFactoryBase
	{
		var json = GetFileResource( path, file );
		if( json is null ) { return null; }
		return JsonHelper.DeserializeJson<T>( ref json, options );
	}

	/// <summary>Returns the Json from a disk file.</summary>
	/// <param name="path">Location of the file.</param>
	/// <param name="file">Name of the file.</param>
	/// <returns>Null is returned if the resource could not be loaded.</returns>
	protected static string? GetFileResource( string path, string file )
	{
		try
		{
			var fi = new FileInfo( Path.Combine( path, file ) );
			if( fi.Exists )
			{
				return File.ReadAllText( fi.FullName, Encoding.ASCII );
			}

		}
		catch( Exception ) { }
		return null;
	}

	/// <summary>Returns the requested number of data objects from a list.</summary>
	/// <typeparam name="T">Data object class or interface.</typeparam>
	/// <param name="data">Collection of data objects.</param>
	/// <param name="count">Number of data objects to return.</param>
	/// <returns>IList collection of data objects.</returns>
	protected static IList<T> ReturnItems<T>( List<T> data, int count )
	{
		int max = data.Count - count;
		if( max > 0 && count < data.Count )
		{
			var idx = sRandom.Next( 1, max + 1 );
			return data.GetRange( idx, count );
		}
		return data;
	}

	/// <summary>Returns the start index for a requested number of data objects.</summary>
	/// <param name="total">Total count of data objects.</param>
	/// <param name="count">Number of data objects to return.</param>
	/// <returns>The starting index.</returns>
	protected static int GetStartIndex( int total, int count )
	{
		int max = total - count;
		if( count > 0 && count <= total )
		{
			return sRandom.Next( 1, max + 1 );
		}
		return 0;
	}
}