using System.Security;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Common.Core.Classes;

/// <summary>Class to provide Json file loading and saving.</summary>
public static class JsonHelper
{
	/// <summary>Reads a Json file and populates an object.</summary>
	/// <typeparam name="T">Generic class or interface.</typeparam>
	/// <param name="fileName">Json file name.</param>
	/// <param name="options">Optional Json serializer options.</param>
	/// <returns>Null is returned if the object could not be populated.</returns>
	public static T? DeserializeFile<T>( string fileName, JsonSerializerOptions? options = null ) where T : class
	{
		T? rtn = null;
		string? json = ReadJsonFromFile( fileName );
		options ??= DefaultSerializerOptions();
		return json is null ? rtn : DeserializeJson<T>( ref json, options );
	}

	/// <summary>Reads a Json string and populates an object.</summary>
	/// <typeparam name="T">Generic class or interface.</typeparam>
	/// <param name="json">Json string.</param>
	/// <param name="options">Optional Json serializer options.</param>
	/// <returns>Null is returned if the object could not be populated.</returns>
	public static T? DeserializeJson<T>( ref string json, JsonSerializerOptions? options = null ) where T : class
	{
		T? rtn = null;
		try
		{
			options ??= DefaultSerializerOptions();
			rtn = JsonSerializer.Deserialize<T>( json, options );
		}
		catch( ArgumentException ) { }
		catch( JsonException ) { }
		catch( NotSupportedException ) { }

		return rtn;
	}

	/// <summary>Populates a list of objects from a Json string.</summary>
	/// <typeparam name="T">Generic class or interface.</typeparam>
	/// <param name="json">Json string.</param>
	/// <param name="options">Optional Json serializer options.</param>
	/// <returns>An empty list is returned if the string could not be converted.</returns>
	public static List<T> DeserializeList<T>( ref string? json, JsonSerializerOptions? options = null )
	{
		if( json is not null )
		{
			options ??= DefaultSerializerOptions();
			try
			{
				List<T>? obj = JsonSerializer.Deserialize<List<T>>( json, options );
				if( obj is not null ) { return obj; }
			}
			catch( ArgumentException ) { }
			catch( JsonException ) { }
			catch( NotSupportedException ) { }
		}

		return new List<T>();
	}

	/// <summary>Reads the Json from a file.</summary>
	/// <param name="fileName">Json file name.</param>
	/// <returns>Null is returned if the file could not be accessed.</returns>
	public static string? ReadJsonFromFile( string fileName )
	{
		if( string.IsNullOrWhiteSpace( fileName ) ) { return null; }
		string? json = null;
		try
		{
			FileInfo fi = new( fileName );
			if( fi.Exists )
			{
				using StreamReader sr = new( fi.FullName );
				json = sr.ReadToEnd();
			}
		}
		catch( ArgumentException ) { }
		catch( NotSupportedException ) { }
		catch( PathTooLongException ) { }
		catch( SecurityException ) { }
		catch( UnauthorizedAccessException ) { }

		return json;
	}

	/// <summary>Returns a collection of settings from a Json applicate settings file.</summary>
	/// <param name="fileName">Json application settings file name.</param>
	/// <param name="section">Application settings section to return <i>(case-sensitive)</i>.</param>
	/// <returns>An empty collection is returned if the settings section could not be found.</returns>
	/// <remarks>If no section is provided it is assumed that the settings are in the root.
	/// <br/>Otherwise it is assumed that the section name is case-sensitive and only 2 levels deep.</remarks>
	public static Dictionary<string, string?> ReadAppSettings( string fileName, string? section )
	{
		Dictionary<string, string?> rtn = new();
		if( string.IsNullOrEmpty( Path.GetDirectoryName( fileName ) ) )
		{
			fileName = ReflectionHelper.AddCurrentPath( fileName );
		}
		string? json = ReadJsonFromFile( fileName );
		if( string.IsNullOrWhiteSpace( json ) ) { return rtn; }
		try
		{
			JsonDocumentOptions options = new JsonDocumentOptions
			{
				AllowTrailingCommas = true,
				MaxDepth = 2,
				CommentHandling = JsonCommentHandling.Skip
			};
			using JsonDocument document = JsonDocument.Parse( json, options );
			JsonElement coll = document.RootElement;
			if( !string.IsNullOrEmpty( section ) ) { coll = coll.GetProperty( section ); }
			foreach( JsonProperty prop in coll.EnumerateObject() )
			{
				if( prop.Value.ValueKind is not JsonValueKind.Object )
				{
					rtn.Add( prop.Name, prop.Value.ToString() );
				}
			}
		}
		catch( ArgumentException ) { }
		catch( InvalidOperationException ) { }
		catch( JsonException ) { }
		catch( KeyNotFoundException ) { }

		return rtn;
	}

	/// <summary>Writes a Json file of the provided object type.</summary>
	/// <typeparam name="T">Generic class or interface.</typeparam>
	/// <param name="obj">Object to save.</param>
	/// <param name="fileName">Json file name.</param>
	/// <param name="options">Optional Json serializer options.</param>
	/// <returns>True if the object was saved.</returns>
	public static bool Serialize<T>( T? obj, string fileName, JsonSerializerOptions? options = null ) where T : class
	{
		if( obj is null || string.IsNullOrWhiteSpace( fileName ) ) { return false; }
		try
		{
			options ??= DefaultSerializerOptions();
			string json = JsonSerializer.Serialize( obj, options );
			if( json is not null )
			{
				File.WriteAllText( fileName, json );
				return true;
			}
		}
		catch( ArgumentException ) { }
		catch( DirectoryNotFoundException ) { }
		catch( IOException ) { }
		catch( NotSupportedException ) { }
		catch( SecurityException ) { }
		catch( UnauthorizedAccessException ) { }

		return false;
	}

	/// <summary>Returns a Json string of the provided object type.</summary>
	/// <typeparam name="T">Generic class or interface.</typeparam>
	/// <param name="obj">Object to serialize.</param>
	/// <param name="options">Optional Json serializer options.</param>
	/// <returns>Null is returned if the serialization fails.</returns>
	public static string? Serialize<T>( T? obj, JsonSerializerOptions? options = null ) where T : class
	{
		if( obj is null ) { return null; }
		try
		{
			options ??= DefaultSerializerOptions();
			return JsonSerializer.Serialize( obj, options );
		}
		catch( NotSupportedException ) { }

		return null;
	}

	/// <summary>Gets a default set of Json Serializer options.</summary>
	/// <returns>
	/// <code>
	/// AllowTrailingCommas = true
	/// IgnoreReadOnlyProperties = true
	/// MaxDepth = 6
	/// PropertyNameCaseInsensitive = true
	/// </code>
	/// </returns>
	public static JsonSerializerOptions DefaultSerializerOptions()
	{
		return new JsonSerializerOptions
		{
			AllowTrailingCommas = true,
			IgnoreReadOnlyProperties = true,
			MaxDepth = 6,
			PropertyNameCaseInsensitive = true,
			Encoder = JavaScriptEncoder.Create( UnicodeRanges.BasicLatin ),
		};
	}
}