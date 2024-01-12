using Common.Core.Models;
using Common.Core.Interfaces;

namespace Common.Data.Api;

/// <summary>This class provides access to Company REST API data.</summary>
public class Companies : Factory, IDataFactory<ICompany>
{
	private static readonly string sResource = nameof( Company ).ToLower();

	#region Constructors

	/// <summary>Initializes a new instance of the Companies class.</summary>
	public Companies() : this( cConfigFile ) { }

	/// <summary>Initializes a new instance of the Companies class using a configuration file.</summary>
	/// <param name="configFile">The name of the configuration file. The default is appsettings.json</param>
	public Companies( string configFile ) : base( cEndpointKey, configFile )
	{
		TotalCount = GetRowCount<Company>( sResource, Company.GetSerializerOptions() );
	}

	private Companies( IList<ICompany> list ) : base( endpointName: string.Empty )
	{
		Data = list is List<ICompany> data ? data : new List<ICompany>();
	}

	#endregion

	/// <summary>Gets or sets the collection of Company objects.</summary>
	public List<ICompany> Data { get; set; } = new List<ICompany>();

	/// <summary>Gets the total number of Companies available.</summary>
	public int TotalCount { get; private set; }

	/// <inheritdoc/>
	/// <summary>Gets a collection of Company objects from the REST API.</summary>
	/// <returns>A collection of Company objects.</returns>
	public IList<ICompany> Get( int max = 0 )
	{
		return GetResource<ICompany>( sResource, Company.GetSerializerOptions(), max, TotalCount );
	}

	/// <inheritdoc/>
	/// <summary>Gets a collection of Company objects from an external Json file.</summary>
	/// <returns>A collection of Company objects.</returns>
	public IList<ICompany> Get( string path, string? file, int max = 0 )
	{
		if( string.IsNullOrWhiteSpace( file ) ) { file = Company.cDefaultFile; }
		if( Data.Count == 0 )
		{
			// Load the data
			Companies? obj = DeserializeJson<Companies>( path, file, Company.GetSerializerOptions() );
			if( obj is not null && obj.Data?.Count > 0 ) { Data = obj.Data; }
		}

		// Return the requested number of objects
		return null == Data ? new List<ICompany>() : ReturnItems( Data, max );
	}

	/// <inheritdoc/>
	public bool Serialize( string path, string? file, IList<ICompany>? list )
	{
		if( string.IsNullOrWhiteSpace( file ) ) { file = Company.cDefaultFile; }
		Companies obj = list is null ? this : new( list );

		// Check that data has been loaded
		return obj.Data is not null && obj.Data.Count > 0 &&
			SerializeJson( obj, path, file, Company.GetSerializerOptions() );
	}

	/// <inheritdoc/>
	/// <summary>Updates an ICompany object with data from another ICompany object.</summary>
	public bool Update( ICompany obj, ICompany mod )
	{
		return PutResource( sResource, obj.Id, mod, Company.GetSerializerOptions() );
	}

	/// <summary>Find a Company.</summary>
	/// <param name="id">Company Id.</param>
	/// <returns>Null is returned if the Company is not found.</returns>
	public static Company? Find( int id )
	{
		Company? company = GetResource<Company>( sResource, Company.GetSerializerOptions(), id );
		if( company is not null ) { return company; }
		return null;
	}
}