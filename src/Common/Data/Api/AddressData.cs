﻿using System.ComponentModel;
using Common.Core.Classes;
using Common.Core.Models;

namespace Common.Data.Api;

/// <summary>Contains data used for Addresses.</summary>
public class AddressData : AddressFactoryBase
{
	private static DataServiceBase? _service;
	private static readonly string sPostcode = typeof( Postcode ).Name.ToLower();

	#region Constructor

	/// <summary>Initializes a new instance of the AddressData class.</summary>
	/// <param name="configFile">The name of the configuration file. The default is appsettings.json</param>
	/// <param name="useAlpha2">Indicates whether to use Alpha-2 ISO Country codes. The default is false.</param>
	/// <param name="countries">Indicates whether ISO Countries should be loaded. The default is true.</param>
	/// <param name="provinces">Indicates whether Provinces should be loaded. The default is true</param>
	/// <param name="postcodes">Indicates whether Postcodes should be loaded. The default is true.</param>
	/// <param name="isoCountry">The ISO-3166 Country code to use for Address data. The default is USA.</param>
	public AddressData( string configFile = DataFactoryBase.cConfigFile, bool useAlpha2 = false,
		bool countries = true, bool provinces = true, bool postcodes = true, string isoCountry = "" )
	{
		_service = Factory.GetDataService( Factory.cEndpointKey, ref configFile );

		if( countries & Countries.Count == 0 )
		{
			LoadCountries( useAlpha2 );
			DefaultCountry = isoCountry;
		}
		if( provinces & Provinces.Count == 0 ) { LoadProvinces(); }
		if( postcodes & Postcodes.Count == 0 ) { LoadPostcodes(); }
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

	private static void LoadProvinces()
	{
		if( _service is null ) { return; }
		string? json = _service.GetResource( typeof( Province ).Name.ToLower() );
		if( json is not null )
		{
			ResultsSet<Province>? obj = JsonHelper.DeserializeJson<ResultsSet<Province>>( ref json );
			if( obj is not null )
			{
				Provinces = obj.Results.ToList();
			}
		}
	}

	private static void LoadPostcodes()
	{
		if( _service is null ) { return; }
		string? json = _service.GetResource( sPostcode );
		if( json is not null )
		{
			ResultsSet<Postcode>? obj = JsonHelper.DeserializeJson<ResultsSet<Postcode>>( ref json );
			if( obj is not null && obj.Total is not null )
			{
				PostcodeCount = obj.Total.Value;
			}
		}
	}

	#endregion

	/// <summary>Gets the information for a requested Postal code.</summary>
	/// <param name="code">Postal Service code.</param>
	/// <returns>Null is returned if the Postcode was not found.</returns>
	public static new Postcode? GetPostcode( string? code )
	{
		Postcode? rtn = AddressFactoryBase.GetPostcode( code );
		if( rtn is not null ) { return rtn; }

		if( _service is null ) { return null; }
		if( code is null || ( DefaultCountry.StartsWith( "US" ) & code.Length < 5 ) ) { return null; }
		if( code.Length > 5 & DefaultCountry.StartsWith( "US" ) ) { code = code[..5]; }

		var json = _service.GetResource( sPostcode + "/" + code );
		if( json is not null )
		{
			var obj = JsonHelper.DeserializeJson<Postcode>( ref json );
			if( obj is not null )
			{
				Postcodes.Add( obj );
				return obj;
			}
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
		List<string?> rtn = [];
		if( _service is null || province is null || province.Length != 2 ) { return rtn; }

		string query = $"{sPostcode}?province={province}";
		List<Postcode>? list = RunQuery( ref query, _service );
		if( list is not null )
		{
			var groups = list.OrderBy( x => x.County ).GroupBy( x => x.County );
			return groups.Select( c => c.Key ).ToList();
		}
		return rtn;
	}

	/// <summary>Gets a sorted list of City names for a requested Province and County.</summary>
	/// <param name="province">Postal Service Province abbreviation.</param>
	/// <param name="county">County name.</param>
	/// <returns>An empty list is returned if the Province code or County name was not found.</returns>
	[EditorBrowsable( EditorBrowsableState.Never )]
	public static IList<string?> GetCityNames( string? province, string? county = null )
	{
		List<string?> rtn = [];
		if( _service is null || province is null ) { return rtn; }

		string query = $"{sPostcode}?province={province}";
		if( !string.IsNullOrWhiteSpace( county ) ) { query += $"&county={Uri.EscapeDataString( county )}"; }
		List<Postcode>? list = RunQuery( ref query, _service );
		if( list is not null )
		{
			var groups = list.OrderBy( x => x.City ).GroupBy( x => x.City );
			return groups.Select( c => c.Key ).ToList();
		}
		return rtn;
	}

	/// <summary>Gets a sorted list of Postal codes for a requested Province, County and City.</summary>
	/// <param name="province">Postal Service Province abbreviation.</param>
	/// <param name="county">County name.</param>
	/// <param name="city">City name.</param>
	/// <returns>An empty list is returned if the Province code, County name, or City name was not found.</returns>
	[EditorBrowsable( EditorBrowsableState.Never )]
	public static IList<string> GetPostcodes( string? province, string? county = null, string? city = null )
	{
		List<string> rtn = [];
		if( _service is null || province is null || province.Length != 2 ) { return rtn; }

		string query = $"{sPostcode}?province={province}";
		if( !string.IsNullOrWhiteSpace( county ) ) { query += $"&county={Uri.EscapeDataString( county )}"; }
		if( !string.IsNullOrWhiteSpace( city ) ) { query += $"&city={Uri.EscapeDataString( city )}"; }

		List<Postcode>? list = RunQuery( ref query, _service );
		if( list is not null )
		{
			return list.OrderBy( x => x.Code ).Select( x => x.Code ).ToList();
		}
		return rtn;
	}

	private static List<Postcode>? RunQuery( ref string query, DataServiceBase service )
	{
		var json = service.GetResource( query );
		if( json is not null )
		{
			var obj = JsonHelper.DeserializeJson<ResultsSet<Postcode>>( ref json );
			if( obj is not null ) { return [.. obj.Results]; }
		}
		return null;
	}

	#endregion
}