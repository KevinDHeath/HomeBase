using Common.Core.Models;
using Common.Core.Interfaces;
using Common.Core.Classes;

namespace Common.Data.SqlServer;

/// <summary>This class provides access to external Company data.</summary>
public class Companies : DataFactoryBase, IDataFactory<ICompany>
{
	#region Constructors

	private readonly FullContextBase? _ctx;

	/// <summary>Initializes a new instance of the Companies class.</summary>
	public Companies()
	{ }

	/// <summary>Initializes a new instance of the Companies class.</summary>
	/// <param name="ctx">Entity Framework DbContext.</param>
	public Companies( FullContextBase ctx )
	{
		_ctx = ctx;
		TotalCount = _ctx.Companies.Count();
	}

	private Companies( IList<ICompany> list )
	{
		Data = list is List<ICompany> data ? data : [];
	}

	#endregion

	/// <summary>Gets or sets the collection of Company objects.</summary>
	public List<ICompany> Data { get; set; } = [];

	/// <summary>Gets the total number of Companies available.</summary>
	public int TotalCount { get; private set; }

	/// <summary>Find a Company.</summary>
	/// <param name="Id">Company Id.</param>
	/// <returns>Null is returned if the Company is not found.</returns>
	public ICompany? Find( int Id )
	{
		if( _ctx is null ) { return null; }
		Company? company = _ctx.Companies.Find( [Id] );
		return company;
	}

	/// <inheritdoc/>
	/// <summary>Gets a collection of Company objects from the Entity Framework.</summary>
	/// <returns>A collection of Company objects.</returns>
	public IList<ICompany> Get( int max = 0 )
	{
		if( _ctx is null || max == 0 ) { return []; }
		int startId = GetStartIndex( _ctx.Companies.Count(), max );
		return [.. _ctx.Companies.Where( c => c.Id > startId ).Take( max )];
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
		return false;
	}
}