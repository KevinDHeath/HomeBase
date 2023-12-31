﻿using Common.Core.Interfaces;
using Common.Core.Models;

namespace Common.Data.Json;

/// <summary>This class provides access to Person Json data.</summary>
public class People : Factory, IDataFactory<IPerson>
{
	#region Constructors

	/// <summary>Initializes a new instance of the Persons class.</summary>
	public People() =>  Data = new List<IPerson>();

	/// <summary>Initializes a new instance of the Persons class using a configuration file.</summary>
	/// <param name="configFile">The name of the configuration file.</param>
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

	/// <inheritdoc/>
	/// <summary>Gets a collection of Person objects from the embedded Json resource.</summary>
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

	/// <inheritdoc/>
	/// <summary>Gets a collection of Person objects from an external Json file.</summary>
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
		return true;
	}
}