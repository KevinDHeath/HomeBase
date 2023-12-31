using System.ComponentModel;
using Common.Core.Classes;
using Common.Core.Models;

namespace Common.Data.Api;

/// <summary>Contains data used for Addresses.</summary>
public class AddressData : AddressFactory
{
	private static DataServiceBase? _service;
	private static readonly string sZipCode = typeof( USZipCode ).Name.ToLower();

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
		_service = Factory.GetDataService( Factory.cEndpointKey, ref configFile );

		if( countries & Countries.Count == 0 ) { LoadCountries( useAlpha2 ); }
		if( usStates & States.Count == 0 ) { LoadStates(); }
		if( usZipCodes & ZipCodes.Count == 0 ) { LoadZipCodes(); }
	}

	private static void LoadCountries( bool useAlpha2 )
	{
		if( _service is null ) { return; }
		if( useAlpha2 ) { UseAlpha2 = useAlpha2; }

		string? json = _service.GetResource( typeof( ISOCountry ).Name.ToLower() );
		if( json is not null )
		{
			ResultsSet<ISOCountry>? obj = JsonHelper.DeserializeJson<ResultsSet<ISOCountry>>( ref json );
			if( obj is not null )
			{
				SetCountries( obj.Results.ToList() );
			}
		}
	}

	private static void LoadStates()
	{
		if( _service is null ) { return; }
		string? json = _service.GetResource( typeof( USState ).Name.ToLower() );
		if( json is not null )
		{
			ResultsSet<USState>? obj = JsonHelper.DeserializeJson<ResultsSet<USState>>( ref json );
			if( obj is not null )
			{
				States = obj.Results.ToList();
			}
		}
	}

	private static void LoadZipCodes()
	{
		if( _service is null ) { return; }
		string? json = _service.GetResource( sZipCode );
		if( json is not null )
		{
			ResultsSet<USZipCode>? obj = JsonHelper.DeserializeJson<ResultsSet<USZipCode>>( ref json );
			if( obj is not null && obj.Total is not null )
			{
				ZipCodeCount = obj.Total.Value;
			}
		}
	}

	#endregion

	/// <summary>Gets the information for a requested US Zip code.</summary>
	/// <param name="usZipCode">5-digit US Postal Service Zip code.</param>
	/// <returns>Null is returned if the Zip code was not found.</returns>
	public static new USZipCode? GetZipCode( string? usZipCode )
	{
		USZipCode? rtn = AddressFactory.GetZipCode( usZipCode );
		if( rtn is not null ) { return rtn; }
		if( _service is null || usZipCode is null || usZipCode.Length < 5 ) { return null; }

		var json = _service.GetResource( sZipCode + "/" + usZipCode );
		if( json is not null )
		{
			var obj = JsonHelper.DeserializeJson<USZipCode>( ref json );
			if( obj is not null )
			{
				ZipCodes.Add( obj );
				return obj;
			}
		}

		return null;
	}

	#region Testing Methods

	/// <summary>Gets a sorted list of County names for a requested US State.</summary>
	/// <param name="state">2-digit US Postal Service State abbreviation.</param>
	/// <returns>An empty list is returned if the State code was not found.</returns>
	[EditorBrowsable( EditorBrowsableState.Never )]
	public static IList<string> GetCountyNames( string? state )
	{
		List<string> rtn = [];
		if( _service is null || state is null || state.Length != 2 ) { return rtn; }

		string query = $"{sZipCode}?state={state}";
		List<USZipCode>? list = RunQuery( ref query, _service );
		if( list is not null )
		{
			var groups = list.OrderBy( x => x.County ).GroupBy( x => x.County );
			return groups.Select( c => c.Key ).ToList();
		}
		return rtn;
	}

	/// <summary>Gets a sorted list of City names for a requested US State and County.</summary>
	/// <param name="state">2-digit US Postal Service State abbreviation.</param>
	/// <param name="county">County name.</param>
	/// <returns>An empty list is returned if the State code or County name was not found.</returns>
	[EditorBrowsable( EditorBrowsableState.Never )]
	public static IList<string> GetCityNames( string? state, string? county = null )
	{
		List<string> rtn = [];
		if( _service is null || state is null || state.Length != 2 ) { return rtn; }

		string query = $"{sZipCode}?state={state}";
		if( !string.IsNullOrWhiteSpace( county ) ) { query += $"&county={Uri.EscapeDataString( county )}"; }
		List<USZipCode>? list = RunQuery( ref query, _service );
		if( list is not null )
		{
			var groups = list.OrderBy( x => x.City ).GroupBy( x => x.City );
			return groups.Select( c => c.Key ).ToList();
		}
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
		List<string> rtn = [];
		if( _service is null || state is null || state.Length != 2 ) { return rtn; }

		string query = $"{sZipCode}?state={state}";
		if( !string.IsNullOrWhiteSpace( county ) ) { query += $"&county={Uri.EscapeDataString( county )}"; }
		if( !string.IsNullOrWhiteSpace( city ) ) { query += $"&city={Uri.EscapeDataString( city )}"; }

		List<USZipCode>? list = RunQuery( ref query, _service );
		if( list is not null )
		{
			return list.OrderBy( x => x.ZipCode ).Select( x => x.ZipCode ).ToList();
		}
		return rtn;
	}

	private static List<USZipCode>? RunQuery( ref string query, DataServiceBase service )
	{
		var json = service.GetResource( query );
		if( json is not null )
		{
			var obj = JsonHelper.DeserializeJson<ResultsSet<USZipCode>>( ref json );
			if( obj is not null ) { return [.. obj.Results]; }
		}
		return null;
	}

	#endregion
}