using Common.Core.Interfaces;
using Common.Core.Models;

namespace Common.Data.Json;

/// <summary>Provides access to embedded Person Json data.</summary>
public class People : Factory, IDataFactory<IPerson>
{
	#region Constructors

	/// <summary>Initializes a new instance of the Persons class.</summary>
	public People() =>  Data = new List<IPerson>();

	/// <summary>Initializes a new instance of the Persons class using a configuration file.</summary>
	/// <param name="configFile">The name of the configuration file.</param>
	/// <remarks>The configuration file is not used as the data is embedded.</remarks>
#pragma warning disable IDE0060 // Remove unused parameter
	public People( string configFile = "" ) => Data = new List<IPerson>();
#pragma warning restore IDE0060 // Remove unused parameter

	private People( IList<IPerson> list )
	{
		Data = list is List<IPerson> data ? data : new List<IPerson>();
	}

	#endregion

	/// <summary>Gets or sets the collection of Person objects.</summary>
	public List<IPerson> Data { get; set; }

	/// <summary>Gets the total number of People available.</summary>
	public int TotalCount { get => Data.Count; }

	/// <summary>Find a Person.</summary>
	/// <param name="Id">Person Id.</param>
	/// <returns><see langword="null"/> is returned if the Person is not found.</returns>
	public IPerson? Find( int Id )
	{
		IPerson? person = Data.Find( c => c.Id == Id );
		if( person is not null ) { return person; }
		return null;
	}

	/// <summary>Gets a collection of Person objects.</summary>
	/// <param name="max">Maximum number of objects to return. Zero indicates all available.</param>
	/// <returns>A collection of Person objects.</returns>
	public IList<IPerson> Get( int max = 0 )
	{
		if( Data?.Count == 0 )
		{
			// Load the data
			People? obj = DeserializeJson<People>( Person.cDefaultFile, Person.GetSerializerOptions() );
			if( obj is not null && obj.Data?.Count > 0 ) { Data = obj.Data; }
		}

		// Return the requested number of objects
		return null == Data ? new List<IPerson>() : ReturnItems( Data, max );
	}

	/// <summary>Gets a collection of Person objects from an external Json file.</summary>
	/// <param name="path">Location of the data file.</param>
	/// <param name="file">Name of the file. If not supplied the default name is used.</param>
	/// <param name="max">Maximum number of objects to return. Zero indicates all available.</param>
	/// <returns>A collection of Person objects.</returns>
	public IList<IPerson> Get( string path, string? file, int max = 0 )
	{
		if( string.IsNullOrWhiteSpace( file ) ) { file = Person.cDefaultFile; }
		if( Data?.Count == 0 )
		{
			// Load the data
			People? obj = DeserializeJson<People>( path, file, Person.GetSerializerOptions() );
			if( obj is not null && obj.Data?.Count > 0 ) { Data = obj.Data; }
		}

		// Return the requested number of objects
		return null == Data ? new List<IPerson>() : ReturnItems( Data, max );
	}

	/// <summary>Serialize a collection of People to a specified file location.</summary>
	/// <param name="path">Location for the file.</param>
	/// <param name="file">Name of the file. If not supplied the default name is used.</param>
	/// <param name="list">The collection to serialize.</param>
	/// <returns><see langword="true"/> if the file was saved, otherwise <see langword="false"/> is returned.</returns>
	/// <remarks>There must be data already loaded and the path must exist.</remarks>
	public bool Serialize( string path, string? file, IList<IPerson>? list )
	{
		if( string.IsNullOrWhiteSpace( file ) ) { file = Person.cDefaultFile; }
		People obj = list is null ? this : new( list );

		// Check that data has been loaded
		return obj.Data is not null && obj.Data.Count > 0 &&
			SerializeJson( obj, path, file, Person.GetSerializerOptions() );
	}

	/// <summary>Updates a Person with data from another of the same kind.</summary>
	/// <param name="obj">Person containing the original values.</param>
	/// <param name="mod">Person containing the modified values.</param>
	/// <returns><see langword="false"/> is returned if there were any failures during the update.</returns>
	/// <remarks><see langword="true"/> is always returned as the data cannot be modified.</remarks>
	public bool Update( IPerson obj, IPerson mod )
	{
		return true;
	}
}