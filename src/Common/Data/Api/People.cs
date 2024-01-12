using Common.Core.Models;
using Common.Core.Interfaces;

namespace Common.Data.Api;

/// <summary>This class provides access to Person REST API data.</summary>
public class People : Factory, IDataFactory<IPerson>
{
	private static readonly string sResource = nameof( Person ).ToLower();

	#region Constructors

	/// <summary>Initializes a new instance of the People class.</summary>
	public People() : this( cConfigFile ) { }

	/// <summary>Initializes a new instance of the People class using a configuration file.</summary>
	/// <param name="configFile">The name of the configuration file. The default is appsettings.json</param>
	public People( string configFile ) : base( cEndpointKey, configFile )
	{
		TotalCount = GetRowCount<Person>( sResource, Person.GetSerializerOptions() );
	}

	private People( IList<IPerson> list ) : base( endpointName: string.Empty )
	{
		Data = list is List<IPerson> data ? data : new List<IPerson>();
	}

	#endregion

	/// <summary>Gets or sets the collection of Person objects.</summary>
	public List<IPerson> Data { get; set; } = new List<IPerson>();

	/// <summary>Gets the total number of People available.</summary>
	public int TotalCount { get; private set; }

	/// <inheritdoc/>
	/// <summary>Gets a collection of Person objects from the REST API.</summary>
	/// <returns>A collection of Person objects.</returns>
	public IList<IPerson> Get( int max = 0 )
	{
		return GetResource<IPerson>( sResource, Person.GetSerializerOptions(), max, TotalCount );
	}

	/// <inheritdoc/>
	/// <summary>Gets a collection of Person objects from an external Json file.</summary>
	/// <returns>A collection of Person objects.</returns>
	public IList<IPerson> Get( string path, string? file, int max = 0 )
	{
		if( string.IsNullOrWhiteSpace( file ) ) { file = Person.cDefaultFile; }
		if( Data.Count == 0 )
		{
			// Load the data
			People? obj = DeserializeJson<People>( path, file, Person.GetSerializerOptions() );
			if( obj is not null && obj.Data?.Count > 0 ) { Data = obj.Data; }
		}

		// Return the requested number of objects
		return null == Data ? new List<IPerson>() : ReturnItems( Data, max );
	}

	/// <inheritdoc/>
	public bool Serialize( string path, string? file, IList<IPerson>? list )
	{
		if( string.IsNullOrWhiteSpace( file ) ) { file = Person.cDefaultFile; }
		People obj = list is null ? this : new( list );

		// Check that data has been loaded
		return obj.Data is not null && obj.Data.Count > 0 &&
			SerializeJson( obj, path, file, Person.GetSerializerOptions() );
	}

	/// <inheritdoc/>
	/// <summary>Updates an IPerson object with data from another IPerson object.</summary>
	public bool Update( IPerson obj, IPerson mod )
	{
		return PutResource( sResource, obj.Id, mod, Person.GetSerializerOptions() );
	}

	/// <summary>Find a Person.</summary>
	/// <param name="id">Person Id.</param>
	/// <returns>Null is returned if the Person is not found.</returns>
	public static Person? Find( int id )
	{
		Person? person = GetResource<Person>( sResource, Person.GetSerializerOptions(), id );
		if( person is not null ) { return person; }
		return null;
	}
}