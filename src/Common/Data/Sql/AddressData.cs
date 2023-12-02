using System.ComponentModel;
using Common.Core.Classes;
using Common.Core.Models;

namespace Common.Data.Sql;

/// <summary>Contains data used for Addresses.</summary>
public class AddressData : AddressFactory
{
	private static string _connString = string.Empty;

	#region Constructor

	/// <summary>Initializes a new instance of the AddressData class.</summary>
	/// <param name="configFile">The name of the configuration file. The default is appsettings.json</param>
	/// <param name="useAlpha2">Indicates whether to use Alpha-2 ISO Country codes. The default is false.</param>
	/// <param name="countries">Indicates whether ISO Countries should be loaded. The default is true.</param>
	/// <param name="usStates">Indicates whether US States should be loaded. The default is true.</param>
	/// <param name="usZipCodes">Indicates whether US Zip Codes should be loaded. The default is true.</param>
	public AddressData( string configFile = DataFactoryBase.cConfigFile, bool useAlpha2 = false,
		bool countries = true, bool usStates = true, bool usZipCodes = true )
	{
		_connString = Factory.SetConnectionString( typeof( AddressData ).Name, configFile );

		if( countries & Countries.Count == 0 ) { LoadCountries( useAlpha2 ); }
		if( usStates & States.Count == 0 ) { LoadStates(); }
		if( usZipCodes & ZipCodes.Count == 0 ) { LoadZipCodes(); }
	}

	private static void LoadCountries( bool useAlpha2 )
	{
		if( string.IsNullOrWhiteSpace( _connString ) ) { return; }
		if( useAlpha2 ) { UseAlpha2 = useAlpha2; }

		List<ISOCountry> list = new();
		string query = "SELECT * FROM [ISOCountries];";
		DataTable? dt = Factory.GetDataTable( ref query , ref _connString );
		if( dt is not null ) { foreach( DataRow row in dt.Rows ) { list.Add( ISOCountry.Read( row ) ); } }
		SetCountries( list );
	}

	private static void LoadStates()
	{
		if( string.IsNullOrWhiteSpace( _connString ) ) { return; }
		States = new List<USState>();
		string query = "SELECT * FROM [USStates];";
		DataTable? dt = Factory.GetDataTable( ref query, ref _connString );
		if( dt is not null ) { foreach( DataRow row in dt.Rows ) { States.Add( USState.Read( row ) ); } }
	}

	private static void LoadZipCodes()
	{
		if( string.IsNullOrWhiteSpace( _connString ) ) { return; }
		ZipCodeCount = Factory.GetRowCount( "USZipCodes", ref _connString );
	}

	#endregion

	/// <summary>Gets the information for a requested US Zip code.</summary>
	/// <param name="usZipCode">5-digit US Postal Service Zip code.</param>
	/// <returns>Null is returned if the Zip code was not found.</returns>
	public static new USZipCode? GetZipCode( string? usZipCode )
	{
		USZipCode? rtn = AddressFactory.GetZipCode( usZipCode );
		if( rtn is not null ) { return rtn; }
		if( string.IsNullOrWhiteSpace( _connString ) || usZipCode is null || usZipCode.Length < 5 ) { return null; }

		string query = $"SELECT * FROM [USZipCodes] WHERE [ZipCode]='{usZipCode}';";
		DataTable? dt = Factory.GetDataTable( ref query, ref _connString );
		if( dt is not null && dt.Rows.Count > 0 )
		{
			rtn = USZipCode.Read( dt.Rows[0] );
			ZipCodes.Add( rtn );
			return rtn;
		}

		return null;
	}

	#region Testing Methods

	/// <summary>Gets a sorted list of County names for a requested US State.</summary>
	/// <param name="code">2-digit US Postal Service State abbreviation.</param>
	/// <returns>An empty list is returned if the State code was not found.</returns>
	[EditorBrowsable( EditorBrowsableState.Never )]
	public static IList<string> GetCountyNames( string? code )
	{
		List<string> rtn = new();
		if( code is null || code.Length != 2 ) { return rtn; }

		string query = $"SELECT [County] FROM [USZipCodes] WHERE [State]='{code}' GROUP BY [County] ORDER BY [County];";
		RunQuery( ref query, rtn );
		return rtn;
	}

	/// <summary>Gets a sorted list of City names for a requested US State and County.</summary>
	/// <param name="state">2-digit US Postal Service State abbreviation.</param>
	/// <param name="county">County name.</param>
	/// <returns>An empty list is returned if the State code or County name was not found.</returns>
	[EditorBrowsable( EditorBrowsableState.Never )]
	public static IList<string> GetCityNames( string? state, string? county = null )
	{
		List<string> rtn = new();
		if( state is null || state.Length != 2 ) { return rtn; }
		state = state.ToUpper() ?? string.Empty;

		string query = $"SELECT [City] FROM [USZipCodes] WHERE [State]='{state}'";
		if( !string.IsNullOrWhiteSpace( county ) ) { query += $" AND [County]='{county}'"; }
		query += $" GROUP BY [City] ORDER BY [City];";
		RunQuery( ref query, rtn );
		return rtn;
	}

	/// <summary>Gets a sorted list of Zip codes for a requested US State, County and City.</summary>
	/// <param name="state">2-digit US Postal Service State abbreviation.</param>
	/// <param name="county">County name.</param>
	/// <param name="city">City name.</param>
	/// <returns>An empty list is returned if the State code, County name, or City name was not found.</returns>
	[EditorBrowsable( EditorBrowsableState.Never )]
	public static IList<string> GetZipCodes( string? state, string? county = null, string? city = null )
	{
		List<string> rtn = new();
		if( state is null || state.Length != 2 ) { return rtn; }
		state = state.ToUpper() ?? string.Empty;

		string query = $"SELECT [ZipCode] FROM [USZipCodes] WHERE [State]='{state}'";
		if( !string.IsNullOrWhiteSpace( county ) ) { query += $" AND [County]='{county}'"; }
		if( !string.IsNullOrWhiteSpace( city ) ) { query += $" AND [City]='{city}'"; }
		query += $" ORDER BY [ZipCode];";
		RunQuery( ref query, rtn );
		return rtn;
	}

	private static void RunQuery( ref string query, List<string> list )
	{
		DataTable? dt = Factory.GetDataTable( ref query, ref _connString );
		if( dt is not null && Factory.FillDataTable( ref query, ref _connString, dt ) )
		{
			foreach( DataRow row in dt.Rows )
			{
				list.Add( row[0].ToString()! );
			}
		}
	}

	#endregion
}