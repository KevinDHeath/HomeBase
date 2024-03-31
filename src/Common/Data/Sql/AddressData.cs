using System.ComponentModel;
using Common.Core.Classes;
using Common.Core.Models;

namespace Common.Data.Sql;

/// <summary>Populates static Address data from a SQL database.</summary>
public class AddressData : AddressFactoryBase
{
	private static string _connString = string.Empty;

	#region Constructor

	/// <summary>Initializes a new instance of the AddressData class.</summary>
	/// <param name="configFile">The name of the configuration file. The default is <c>appsettings.json</c></param>
	/// <param name="useAlpha2">Indicates whether to use Alpha-2 ISO Country codes. The default is <see langword="false"/>.</param>
	/// <param name="isoCountry">The ISO-3166 Country code to use for Address data. The default is <c>USA</c>.</param>
	/// <param name="countries">Indicates whether ISO Countries should be loaded. The default is <see langword="true"/>.</param>
	/// <param name="provinces">Indicates whether Provinces should be loaded. The default is <see langword="true"/>.</param>
	/// <param name="postcodes">Indicates whether Postcodes should be loaded. The default is <see langword="true"/>.</param>
	public AddressData( string configFile = DataFactoryBase.cConfigFile, bool useAlpha2 = false,
		string isoCountry = "", bool countries = true, bool provinces = true, bool postcodes = true )
	{
		_connString = Factory.SetConnectionString( typeof( AddressData ).Name, configFile );

		if( countries )
		{
			LoadCountries( useAlpha2 );
			DefaultCountry = isoCountry;
		}
		if( provinces ) { LoadProvinces(); }
		if( postcodes ) { LoadPostcodes(); }
	}

	private static void LoadCountries( bool useAlpha2 )
	{
		Countries = new List<CountryCode>();

		if( string.IsNullOrWhiteSpace( _connString ) ) { return; }
		if( useAlpha2 ) { UseAlpha2 = useAlpha2; }

		List<ISOCountry> list = new();
		string query = "SELECT * FROM [ISOCountries];";
		DataTable? dt = Factory.GetDataTable( ref query , ref _connString );
		if( dt is not null ) { foreach( DataRow row in dt.Rows ) { list.Add( ISOCountry.Read( row ) ); } }
		SetCountries( list );
	}

	private static void LoadProvinces()
	{
		Provinces = new List<Province>();

		if( string.IsNullOrWhiteSpace( _connString ) ) { return; }
		Provinces = new List<Province>();
		string query = "SELECT * FROM [Provinces];";
		DataTable? dt = Factory.GetDataTable( ref query, ref _connString );
		if( dt is not null ) { foreach( DataRow row in dt.Rows ) { Provinces.Add( Province.Read( row ) ); } }
	}

	private static void LoadPostcodes()
	{
		Postcodes = new List<Postcode>();
		PostcodeCount = 0;

		if( string.IsNullOrWhiteSpace( _connString ) ) { return; }
		PostcodeCount = Factory.GetRowCount( "Postcodes", ref _connString );
	}

	#endregion

	/// <summary>Gets the information for a Postcode.</summary>
	/// <param name="code">Postal Service code.</param>
	/// <returns><see langword="null"/> is returned if the Postcode is not found.</returns>
	/// <remarks>The postcode is added to the cache the first time it is referenced.</remarks>
	public static new Postcode? GetPostcode( string? code )
	{
		Postcode? rtn = AddressFactoryBase.GetPostcode( code );
		if( rtn is not null ) { return rtn; }

		if( string.IsNullOrWhiteSpace( _connString ) ) { return null; }
		if( code is null || ( DefaultCountry.StartsWith( "US" ) & code.Length < 5 ) ) { return null; }
		if( code.Length > 5 & DefaultCountry.StartsWith( "US" ) ) { code = code[..5]; }

		// Try to get the Postcode from the database
		string query = $"SELECT * FROM [Postcodes] WHERE [Code]='{code}';";
		DataTable? dt = Factory.GetDataTable( ref query, ref _connString );
		if( dt is not null && dt.Rows.Count > 0 )
		{
			rtn = Postcode.Read( dt.Rows[0] );
			Postcodes.Add( rtn );
			return rtn;
		}

		return null;
	}

	#region Testing Methods

	/// <summary>Gets a sorted list of County names for a requested Province.</summary>
	/// <param name="province">Postal Service Province abbreviation.</param>
	/// <returns>An empty list is returned if the Province code was not found.</returns>
	[EditorBrowsable( EditorBrowsableState.Never )]
	public static IList<string?> GetCountyNames( string? province )
	{
		List<string?> list = new();
		if( province is null || string.IsNullOrWhiteSpace( province ) ) { return list; }
		province = province.Trim();

		string query = $"SELECT [County] FROM [Postcodes] WHERE [Province]='{province}' GROUP BY [County] ORDER BY [County];";

		RunQuery( ref query, list );
		return list;
	}

	/// <summary>Gets a sorted list of City names for a requested Province and County.</summary>
	/// <param name="province">Postal Service Province abbreviation.</param>
	/// <param name="county">County name.</param>
	/// <returns>An empty list is returned if the Province code or County name was not found.</returns>
	[EditorBrowsable( EditorBrowsableState.Never )]
	public static IList<string?> GetCityNames( string? province, string? county = null )
	{
		List<string?> list = new();
		if( province is null || string.IsNullOrWhiteSpace( province ) ) { return list; }
		province = province.Trim();

		string query = $"SELECT [City] FROM [Postcodes] WHERE [Province]='{province}'";
		if( !string.IsNullOrWhiteSpace( county ) ) { query += $" AND [County]='{county}'"; }
		query += $" GROUP BY [City] ORDER BY [City];";

		RunQuery( ref query, list );
		return list;
	}

	/// <summary>Gets a sorted list of Zip codes for a requested Province, County and City.</summary>
	/// <param name="province">Postal Service Province abbreviation.</param>
	/// <param name="county">County name.</param>
	/// <param name="city">City name.</param>
	/// <returns>An empty list is returned if the State code, County name, or City name was not found.</returns>
	[EditorBrowsable( EditorBrowsableState.Never )]
	public static IList<string?> GetPostcodes( string? province, string? county = null, string? city = null )
	{
		List<string?> list = new();
		if( province is null || string.IsNullOrWhiteSpace( province ) ) { return list; }
		province = province.ToUpper() ?? string.Empty;

		string query = $"SELECT [Code] FROM [Postcodes] WHERE [Province]='{province}'";
		if( !string.IsNullOrWhiteSpace( county ) ) { query += $" AND [County]='{county}'"; }
		if( !string.IsNullOrWhiteSpace( city ) ) { query += $" AND [City]='{city}'"; }
		query += $" ORDER BY [Code];";

		RunQuery( ref query, list );
		return list;
	}

	private static void RunQuery( ref string query, List<string?> list )
	{
		DataTable? dt = Factory.GetDataTable( ref query, ref _connString );
		if( dt is not null && Factory.FillDataTable( ref query, ref _connString, dt ) )
		{
			foreach( DataRow row in dt.Rows )
			{
				list.Add( row[0].ToString() );
			}
		}
	}

	#endregion
}