using Common.Core.Interfaces;
using Common.Core.Models;

namespace Common.Data.Json;

/// <summary>Provides access to embedded Company Json data.</summary>
public class Companies : Factory, IDataFactory<ICompany>
{
	#region Constructors

	/// <summary>Initializes a new instance of the Companies class.</summary>
	public Companies() { }

	/// <summary>Initializes a new instance of the Companies class using a configuration file.</summary>
	/// <param name="configFile">The name of the configuration file.</param>
	/// <remarks>The configuration file is not used as the data is embedded.</remarks>
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

	/// <summary>Find a Company.</summary>
	/// <param name="Id">Company Id.</param>
	/// <returns><see langword="null"/> is returned if the Company is not found.</returns>
	public ICompany? Find( int Id )
	{
		ICompany? company = Data.Find( c => c.Id == Id );
		if( company is not null ) { return company; }
		return null;
	}

	/// <summary>Gets a collection of Company objects.</summary>
	/// <param name="max">Maximum number of objects to return. Zero indicates all available.</param>
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

	/// <summary>Gets a collection of Company objects from an external Json file.</summary>
	/// <param name="path">Location of the data file.</param>
	/// <param name="file">Name of the file. If not supplied the default name is used.</param>
	/// <param name="max">Maximum number of objects to return. Zero indicates all available.</param>
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

	/// <summary>Serialize a collection of Companies to a specified file location.</summary>
	/// <param name="path">Location for the file.</param>
	/// <param name="file">Name of the file. If not supplied the default name is used.</param>
	/// <param name="list">The collection to serialize.</param>
	/// <returns><see langword="true"/> if the file was saved, otherwise <see langword="false"/> is returned.</returns>
	/// <remarks>There must be data already loaded and the path must exist.</remarks>
	public bool Serialize( string path, string? file, IList<ICompany>? list )
	{
		if( string.IsNullOrWhiteSpace( file ) ) { file = Company.cDefaultFile; }
		Companies obj = list is null ? this : new( list );

		// Check that data has been loaded
		return obj.Data is not null && obj.Data.Count > 0 &&
			SerializeJson( obj, path, file, Company.GetSerializerOptions() );
	}

	/// <summary>Updates a Company with data from another of the same kind.</summary>
	/// <param name="obj">Company containing the original values.</param>
	/// <param name="mod">Company containing the modified values.</param>
	/// <returns><see langword="false"/> is returned if there were any failures during the update.</returns>
	/// <remarks><see langword="true"/> is always returned as the data cannot be modified.</remarks>
	public bool Update( ICompany obj, ICompany mod )
	{
		return true;
	}
}