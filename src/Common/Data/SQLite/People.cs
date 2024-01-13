using Common.Core.Models;
using Common.Core.Interfaces;
using Common.Core.Classes;

namespace Common.Data.SQLite;

/// <summary>This class provides access to external Person data.</summary>
public class People : DataFactoryBase, IDataFactory<IPerson>
{
	#region Constructors

	private readonly EntityContextBase? _ctx;

	/// <summary>Initializes a new instance of the People class.</summary>
	public People()
	{ }

	/// <summary>Initializes a new instance of the People class.</summary>
	/// <param name="ctx">Entity Framework DbContext.</param>
	public People( EntityContextBase ctx )
	{
		_ctx = ctx;
		TotalCount = _ctx.People.Count();
	}

	private People( IList<IPerson> list )
	{
		Data = list is List<IPerson> data ? data : [];
	}

	#endregion

	/// <summary>Gets or sets the collection of Person objects.</summary>
	public List<IPerson> Data { get; set; } = [];

	/// <summary>Gets the total number of People available.</summary>
	public int TotalCount { get; private set; }

	/// <summary>Find a Person.</summary>
	/// <param name="Id">Person Id.</param>
	/// <returns>Null is returned if the Person is not found.</returns>
	public IPerson? Find( int Id )
	{
		if( _ctx is null ) { return null; }
		Person? person = _ctx.People.Find( [Id] );
		return person;
	}

	/// <inheritdoc/>
	/// <summary>Gets a collection of Person objects from the Entity Framework.</summary>
	/// <returns>A collection of Person objects.</returns>
	public IList<IPerson> Get( int max = 0 )
	{
		if( _ctx is null || max == 0 ) { return []; }
		int startId = GetStartIndex( _ctx.People.Count(), max );
		return [.. _ctx.People.Where( c => c.Id > startId ).Take( max )];
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
		return false;
	}
}