using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Common.Core.Models;
using Common.Core.Classes;

namespace Common.Data.SQLite;

/// <summary>Contains data used for Addresses.</summary>
public class AddressData : AddressFactoryBase
{
	#region Constructor

	/// <summary>Initializes a new instance of the AddressData class.</summary>
	/// <param name="useAlpha2">Indicates whether to use Alpha-2 ISO Country codes. The default is false.</param>
	/// <param name="countries">Indicates whether ISO Countries should be loaded. The default is true.</param>
	/// <param name="provinces">Indicates whether Province should be loaded. The default is true.</param>
	/// <param name="postcodes">Indicates whether Postcodes should be loaded. The default is true.</param>
	public AddressData( bool useAlpha2 = false, bool countries = true, bool provinces = true, bool postcodes = true )
	{
		using AddressContextBase context = new();
		if( countries & Countries.Count == 0 ) { LoadCountries( useAlpha2, context ); }
		if( provinces & Provinces.Count == 0 ) { LoadStates( context ); }
		if( postcodes & Postcodes.Count == 0 ) { LoadZipCodes( context ); }
	}

	private static void LoadCountries( bool useAlpha2, AddressContextBase context )
	{
		if( useAlpha2 ) { UseAlpha2 = useAlpha2; }

		SetCountries( context.ISOCountries.ToList() );
	}

	private static void LoadStates( AddressContextBase context )
	{
		Provinces = context.Provinces.ToList();
	}

	private static void LoadZipCodes( AddressContextBase context )
	{
		PostcodeCount = context.Postcodes.Count();
	}

	#endregion

	/// <summary>Gets the information for a requested US Zip code.</summary>
	/// <param name="postcode">5-digit US Postal Service Zip code.</param>
	/// <returns>Null is returned if the Zip code was not found.</returns>
	public static new Postcode? GetPostcode( string? postcode )
	{
		Postcode? rtn = AddressFactoryBase.GetPostcode( postcode );
		if( rtn is not null ) { return rtn; }

		// Try to get the zip code from the data context
		if( postcode is null || postcode.Length < 5 ) { return null; }
		using AddressContextBase context = new();
		Postcode? code = context.Postcodes.FirstOrDefault( z => z.Code == postcode );
		if( code is not null )
		{
			Postcodes.Add( code );
			return code;
		}
		return null;
	}

	#region Testing Methods

	/// <summary>Gets a sorted list of County names for a requested US State.</summary>
	/// <param name="code">2-digit US Postal Service State abbreviation.</param>
	/// <returns>An empty list is returned if the State code was not found.</returns>
	[EditorBrowsable( EditorBrowsableState.Never )]
	public static IList<string?> GetCountyNames( string? code )
	{
		List<string?> rtn = [];
		if( code is null || code.Length != 2 ) { return rtn; }

		FormattableString query = $"SELECT [County] FROM [Postcodes] WHERE [State]='{code}' GROUP BY [County] ORDER BY [County];";
		using AddressContextBase context = new();
		return context.Postcodes.FromSql( query ).Select( z => z.County ).ToList();
	}

	/// <summary>Gets a sorted list of City names for a requested US State and County.</summary>
	/// <param name="state">2-digit US Postal Service State abbreviation.</param>
	/// <param name="county">County name.</param>
	/// <returns>An empty list is returned if the State code or County name was not found.</returns>
	[EditorBrowsable( EditorBrowsableState.Never )]
	public static IList<string?> GetCityNames( string? state, string? county = null )
	{
		List<string?> rtn = [];
		if( state is null || state.Length != 2 ) { return rtn; }
		state = state.ToUpper() ?? string.Empty;

		string and = string.Empty;
		if( !string.IsNullOrWhiteSpace( county ) ) { and = $" AND [County]='{county}'"; }

		FormattableString query = $"SELECT [City] FROM [Postcodes] WHERE [State]='{state}'{and} GROUP BY [City] ORDER BY [City];";
		using AddressContextBase context = new();
		return context.Postcodes.FromSql( query ).Select( z => z.City ).ToList();
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
		if( state is null || state.Length != 2 ) { return rtn; }
		state = state.ToUpper() ?? string.Empty;

		string addCounty = string.Empty, addCity = string.Empty;
		if( !string.IsNullOrWhiteSpace( county ) ) { addCounty += $" AND [County]='{county}'"; }
		if( !string.IsNullOrWhiteSpace( city ) ) { addCity += $" AND [City]='{city}'"; }

		FormattableString query = $"SELECT [ZipCode] FROM [USZipCodes] WHERE [State]='{state}'{addCounty}{addCity} ORDER BY [ZipCode];";
		using AddressContextBase context = new();
		return context.Postcodes.FromSql( query ).Select( z => z.Code ).ToList();
	}

	#endregion
}