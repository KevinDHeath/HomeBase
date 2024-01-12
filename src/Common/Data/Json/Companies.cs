using Common.Core.Interfaces;
using Common.Core.Models;

namespace Common.Data.Json;

/// <summary>This class provides access to Company Json data.</summary>
public class Companies : Factory, IDataFactory<ICompany>
{
	#region Constructors

	/// <summary>Initializes a new instance of the Companies class.</summary>
	public Companies() { }

	/// <summary>Initializes a new instance of the Companies class using a configuration file.</summary>
	/// <param name="configFile">The name of the configuration file.</param>
#pragma warning disable IDE0060 // Remove unused parameter
	public Companies( string configFile = "" ) { }
#pragma warning restore IDE0060 // Remove unused parameter

	private Companies( IList<ICompany> list )
	{
		Data = list is List<ICompany> data ? data : new List<ICompany>();
	}

	#endregion

	/// <summary>Gets or sets the collection of Company objects.</summary>
	public List<ICompany> Data { get; set; } = new List<ICompany>();

	/// <summary>Gets the total number of Companies available.</summary>
	public int TotalCount { get => Data.Count; }

	/// <inheritdoc/>
	/// <summary>Gets a collection of Company objects from the embedded Json resource.</summary>
	/// <returns>A collection of Company objects.</returns>
	public IList<ICompany> Get( int max = 0 )
	{
		if( Data?.Count == 0 )
		{
			// Load the data
			Companies? obj = DeserializeJson<Companies>( Company.cDefaultFile, Company.GetSerializerOptions() );
			if( obj is not null && obj.Data?.Count > 0 ) { Data = obj.Data; }
		}

		// Return the requested number of objects
		return null == Data ? new List<ICompany>() : ReturnItems( Data, max );
	}

	/// <inheritdoc/>
	/// <summary>Gets a collection of Company objects from an external Json file.</summary>
	/// <returns>A collection of Company objects.</returns>
	public IList<ICompany> Get( string path, string? file, int max = 0 )
	{
		if( string.IsNullOrWhiteSpace( file ) ) { file = Company.cDefaultFile; }
		if( Data?.Count == 0 )
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
		return true;
	}

	/// <summary>Find a Company.</summary>
	/// <param name="id">Company Id.</param>
	/// <returns>Null is returned if the Company is not found.</returns>
	public ICompany? Find( int id )
	{
		ICompany? company = Data.Find( c => c.Id == id );
		if( company is not null ) { return company; }
		return null;
	}
}