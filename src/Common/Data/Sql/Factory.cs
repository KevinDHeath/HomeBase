global using System.Data;
using Microsoft.Data.SqlClient;
using Common.Core.Classes;

namespace Common.Data.Sql;

/// <summary>Base class to provide common SQL database access functionality.</summary>
/// <remarks>This uses connections strings contained in a Json application settings file.<br/>
/// The default file name is "appsettings.json".
/// <code language="JSON">
/// {
///   "ConnectionStrings": {
///     "AddressData": "Server=XX;Database=YY;Trusted_Connection=true;TrustServerCertificate=true;",
///     "CompanyData": "Server=XX;Database=YY;Trusted_Connection=true;TrustServerCertificate=true;",
///     "PersonData": "Server=XX;Database=YY;Trusted_Connection=true;TrustServerCertificate=true;"
///   }
/// }
/// </code>
/// </remarks>
public abstract class Factory : DataFactoryBase
{
	/// <summary>The database tables Address prefix.</summary>
	protected const string cAddressPrefix = "Address_";
	private static string? connectionsSection = "ConnectionStrings";

	#region Constructor and Initialization

	/// <summary>The SQL database connection string.</summary>
	protected string _connString;

	/// <summary>Initializes a new instance of the Factory class.</summary>
	/// <param name="connectionStringName">The connection string settings name.</param>
	/// <param name="configFile">The name of the configuration file.</param>
	protected Factory( string connectionStringName, string configFile = "" )
	{
		if( string.IsNullOrWhiteSpace( configFile ) ) { configFile = cConfigFile; }
		_connString = SetConnectionString( connectionStringName, configFile );
	}

	internal static string SetConnectionString( string connectionStringName, string configFile )
	{
		if( string.IsNullOrWhiteSpace( connectionStringName ) ) { return string.Empty; }

		Dictionary<string, string?> config = JsonHelper.ReadAppSettings( ref configFile, ref connectionsSection );
		config.TryGetValue( connectionStringName, out string? setting );
		if( !string.IsNullOrWhiteSpace( setting ) )
		{
			try
			{
				var builder = new SqlConnectionStringBuilder( setting );
				return builder.ConnectionString;
			}
			catch( Exception ) { }
		}
		return string.Empty;
	}

	#endregion

	#region Internal Methods

	internal static bool FillDataTable( ref string query, ref string connString, DataTable dataTable )
	{
		return ExecuteFill( ref query, ref connString, dataTable );
	}

	internal static DataTable? GetDataTable( ref string query, ref string connString )
	{
		DataTable dataTable = new();
		bool ok = FillDataTable( ref query, ref connString, dataTable );
		return ok ? dataTable : null;
	}

	internal static int GetRowCount( string tableName, ref string connString )
	{
		if( string.IsNullOrWhiteSpace( tableName ) ) { return -1; }

		string query = $"SELECT COUNT(*) FROM [{tableName}];";
		object res = ExecuteScaler( ref query, ref connString );
		if( res is int i && i > -1 ) { return i; }
		return 0;
	}

	internal static int UpdateTable( ref string sql, ref string connString )
	{
		int res = ExecuteNonQuery( ref sql, ref connString );
		return res < 0 ? 0 : res;
	}

	#endregion

	#region Private Methods

	private static bool ExecuteFill( ref string query, ref string connString, DataTable dataTable )
	{
		if( string.IsNullOrWhiteSpace( connString ) ) { return false; }
		if( dataTable.Rows.Count > 0 ) { dataTable.Rows.Clear(); }
		try
		{
			using SqlConnection conn = new( connString );
			using SqlCommand cmd = new( query, conn );
			using SqlDataAdapter adapter = new( cmd );
			{
				cmd.Connection.Open();
				adapter.Fill( dataTable );
				return true;
			}
		}
		catch( Exception ) { return false; }
	}

	private static object ExecuteScaler( ref string sql, ref string connString )
	{
		if( string.IsNullOrWhiteSpace( connString ) ) { return 0; }
		try
		{
			using SqlConnection conn = new( connString );
			using SqlCommand cmd = new( sql, conn );
			{
				cmd.Connection.Open();
				return cmd.ExecuteScalar();
			}
		}
		catch( Exception ) { return -1; }
	}

	private static int ExecuteNonQuery( ref string sql, ref string connString )
	{
		if( string.IsNullOrWhiteSpace( connString ) ) { return -1; }
		try
		{
			using SqlConnection conn = new( connString );
			using SqlCommand cmd = new( sql, conn );
			{
				cmd.Connection.Open();
				return cmd.ExecuteNonQuery();
			}
		}
		catch( Exception ) { return -1; }
	}

	#endregion
}