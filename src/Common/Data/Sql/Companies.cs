using Common.Core.Models;
using Common.Core.Interfaces;

namespace Common.Data.Sql;

/// <summary>This class provides access to Company SQL data.</summary>
public class Companies : Factory, IDataFactory<ICompany>
{
	private static readonly DataTable _dataTable = new( tableName: "Companies" );
	private const string cConnectionKey = "CompanyData";

	#region Constructors

	/// <summary>Initializes a new instance of the Companies class.</summary>
	public Companies() : this( cConfigFile ) { }

	/// <summary>Initializes a new instance of the Companies class using a configuration file.</summary>
	/// <param name="configFile">The name of the configuration file. The default is appsettings.json</param>
	public Companies( string configFile ) : base( cConnectionKey, configFile )
	{
		TotalCount = GetRowCount( _dataTable.TableName, ref _connString );
	}

	private Companies( IList<ICompany> list ) : base( connectionStringName: string.Empty )
	{
		Data = list is List<ICompany> data ? data : new List<ICompany>();
	}

	#endregion

	/// <summary>Gets or sets the collection of Company objects.</summary>
	public List<ICompany> Data { get; set; } = new List<ICompany>();

	/// <summary>Gets the total number of Companies available.</summary>
	public int TotalCount { get; private set; }

	/// <inheritdoc/>
	/// <summary>Gets a collection of Company objects from the SQL database table.</summary>
	/// <returns>A collection of Company objects.</returns>
	public IList<ICompany> Get( int max = 0 )
	{
		string query = $"SELECT * FROM [{_dataTable.TableName}]";
		if( max > 0 )
		{
			int start = GetStartIndex( TotalCount, max );
			query += $" WHERE [Id] > {start} AND [Id] <= {start + max}";
		}

		List<ICompany> rtn = new();
		if( FillDataTable( ref query, ref _connString, _dataTable ) )
		{
			foreach( DataRow row in _dataTable.Rows )
			{
				Company item = Company.Read( row, cAddressPrefix );
				Data.Add( item );
				rtn.Add( item );
			}
		}
		return rtn;
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
		string sql = $"SELECT * FROM [{_dataTable.TableName}] WHERE [Id]={obj.Id};";
		if( FillDataTable( ref sql, ref _connString, _dataTable ) && _dataTable.Rows.Count == 1 )
		{
			string cols = Company.UpdateSQL( _dataTable.Rows[0], obj, mod, cAddressPrefix );
			if( cols.Length > 0 )
			{
				sql = $"UPDATE [{_dataTable.TableName}] SET {cols} WHERE [Id]={obj.Id};";
				_ = UpdateTable( ref sql, ref _connString );
			}
			return true;
		}
		return false;
	}

	/// <summary>Find a Company for a given Id.</summary>
	/// <param name="id">Id of the Company.</param>
	/// <returns>Null is returned if the Company is not found.</returns>
	public Company? Find( int id )
	{
		string sql = $"SELECT * FROM [{_dataTable.TableName}] WHERE [Id]={id};";
		if( FillDataTable( ref sql, ref _connString, _dataTable ) && _dataTable.Rows.Count == 1 )
		{
			return Company.Read( _dataTable.Rows[0], cAddressPrefix );
		}
		return null;
	}
}