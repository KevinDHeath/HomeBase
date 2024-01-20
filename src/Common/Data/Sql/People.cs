using Common.Core.Models;
using Common.Core.Interfaces;

namespace Common.Data.Sql;

/// <summary>Provides access to SQL Person data.</summary>
public class People : Factory, IDataFactory<IPerson>
{
	private static readonly DataTable _dataTable = new( tableName: "People" );
	private const string cConnectionKey = "PersonData";

	#region Constructors

	/// <summary>Initializes a new instance of the People class.</summary>
	public People() : this( cConfigFile ) { }

	/// <summary>Initializes a new instance of the People class using a configuration file.</summary>
	/// <param name="configFile">The name of the configuration file. The default is <c>appsettings.json</c></param>
	public People( string configFile ) : base( cConnectionKey, configFile )
	{
		TotalCount = GetRowCount( _dataTable.TableName, ref _connString );
	}

	private People( IList<IPerson> list ) : base( connectionStringName: string.Empty )
	{
		Data = list is List<IPerson> data ? data : new List<IPerson>();
	}

	#endregion

	/// <summary>Gets or sets the collection of Person objects.</summary>
	public List<IPerson> Data { get; set; } = new List<IPerson>();

	/// <summary>Gets the total number of People available.</summary>
	public int TotalCount { get; private set; }

	/// <summary>Finds a Person in the collection.</summary>
	/// <param name="Id">Person Id.</param>
	/// <returns><see langword="null"/> is returned if the Person is not found.</returns>
	public IPerson? Find( int Id )
	{
		string sql = $"SELECT * FROM [{_dataTable.TableName}] WHERE [Id]={Id};";
		if( FillDataTable( ref sql, ref _connString, _dataTable ) && _dataTable.Rows.Count == 1 )
		{
			return Person.Read( _dataTable.Rows[0], cAddressPrefix );
		}
		return null;
	}

	/// <summary>Gets a collection of Person objects.</summary>
	/// <param name="max">Maximum number of objects to return. Zero indicates all available.</param>
	/// <returns>A collection of Person objects.</returns>
	public IList<IPerson> Get( int max = 0 )
	{
		string query = $"SELECT * FROM [{_dataTable.TableName}]";
		if( max > 0 )
		{
			int start = GetStartIndex( TotalCount, max );
			query += $" WHERE [Id] > {start} AND [Id] <= {start + max}";
		}

		List<IPerson> rtn = new();
		if( FillDataTable( ref query, ref _connString, _dataTable ) )
		{
			foreach( DataRow row in _dataTable.Rows )
			{
				Person item = Person.Read( row, cAddressPrefix );
				Data.Add( item );
				rtn.Add( item );
			}
		}
		return rtn;
	}

	/// <summary>Gets a collection of Person objects from an external Json file.</summary>
	/// <param name="path">Location of the data file.</param>
	/// <param name="file">Name of the file. If not supplied the default name is used.</param>
	/// <param name="max">Maximum number of objects to return. Zero indicates all available.</param>
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
	public bool Update( IPerson obj, IPerson mod )
	{
		string sql = $"SELECT * FROM [{_dataTable.TableName}] WHERE [Id]={obj.Id};";
		if( FillDataTable( ref sql, ref _connString, _dataTable ) && _dataTable.Rows.Count == 1 )
		{
			string cols = Person.UpdateSQL( _dataTable.Rows[0], obj, mod, cAddressPrefix );
			if( cols.Length > 0 )
			{
				sql = $"UPDATE [{_dataTable.TableName}] SET {cols} WHERE [Id]={obj.Id};";
				_ = UpdateTable( ref sql, ref _connString );
			}
			return true;
		}
		return false;
	}
}