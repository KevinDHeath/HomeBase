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
	/// <param name="provinces">Indicates whether Provinces should be loaded. The default is true.</param>
	/// <param name="postcodes">Indicates whether Postcodes should be loaded. The default is true.</param>
	public AddressData( bool useAlpha2 = false, bool countries = true, bool provinces = true, bool postcodes = true )
	{
		using AddressContextBase context = new();
		if( countries & Countries.Count == 0 ) { LoadCountries( useAlpha2, context ); }
		if( provinces & Provinces.Count == 0 ) { LoadProvinces( context ); }
		if( postcodes & Postcodes.Count == 0 ) { LoadPostcodes( context ); }
	}

	private static void LoadCountries( bool useAlpha2, AddressContextBase context )
	{
		if( useAlpha2 ) { UseAlpha2 = useAlpha2; }

		SetCountries( context.ISOCountries.ToList() );
	}

	private static void LoadProvinces( AddressContextBase context )
	{
		Provinces = context.Provinces.ToList();
	}

	private static void LoadPostcodes( AddressContextBase context )
	{
		PostcodeCount = context.Postcodes.Count();
	}

	#endregion

	/// <summary>Gets the information for a requested Postcode.</summary>
	/// <param name="code">Postal Service code.</param>
	/// <returns>Null is returned if the Postcode was not found.</returns>
	public static new Postcode? GetPostcode( string? code )
	{
		Postcode? rtn = AddressFactoryBase.GetPostcode( code );
		if( rtn is not null ) { return rtn; }

		if( code is null || ( DefaultCountry.StartsWith( "US" ) & code.Length < 5 ) ) { return null; }
		if( code.Length > 5 & DefaultCountry.StartsWith( "US" ) ) { code = code[..5]; }

		// Try to get the zip code from the data context
		using AddressContextBase context = new();
		Postcode? postCode = context.Postcodes.FirstOrDefault( z => z.Code == code );
		if( postCode is not null )
		{
			Postcodes.Add( postCode );
			return postCode;
		}
		return null;
	}

	#region Testing Methods

	/// <summary>Gets a sorted list of County names for a requested Province.</summary>
	/// <param name="code">Postal Service Province abbreviation.</param>
	/// <returns>An empty list is returned if the Province code was not found.</returns>
	[EditorBrowsable( EditorBrowsableState.Never )]
	public static IList<string?> GetCountyNames( string? code )
	{
		List<string?> rtn = [];
		if( code is null || code.Length != 2 ) { return rtn; }

		FormattableString query = $"SELECT [County] FROM [Postcodes] WHERE [Province]='{code}' GROUP BY [County] ORDER BY [County];";
		using AddressContextBase context = new();
		return context.Postcodes.FromSql( query ).Select( z => z.County ).ToList();
	}

	/// <summary>Gets a sorted list of City names for a requested Province and County.</summary>
	/// <param name="province">Postal Service Province abbreviation.</param>
	/// <param name="county">County name.</param>
	/// <returns>An empty list is returned if the Province code or County name was not found.</returns>
	[EditorBrowsable( EditorBrowsableState.Never )]
	public static IList<string?> GetCityNames( string? province, string? county = null )
	{
		List<string?> rtn = [];
		if( province is null ) { return rtn; }
		province = province.ToUpper() ?? string.Empty;

		string and = string.Empty;
		if( !string.IsNullOrWhiteSpace( county ) ) { and = $" AND [County]='{county}'"; }

		FormattableString query = $"SELECT [City] FROM [Postcodes] WHERE [Province]='{province}'{and} GROUP BY [City] ORDER BY [City];";
		using AddressContextBase context = new();
		return context.Postcodes.FromSql( query ).Select( z => z.City ).ToList();
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
		if( province is null ) { return rtn; }
		province = province.ToUpper() ?? string.Empty;

		string addCounty = string.Empty, addCity = string.Empty;
		if( !string.IsNullOrWhiteSpace( county ) ) { addCounty += $" AND [County]='{county}'"; }
		if( !string.IsNullOrWhiteSpace( city ) ) { addCity += $" AND [City]='{city}'"; }

		FormattableString query = $"SELECT [Code] FROM [Postcodes] WHERE [Province]='{province}'{addCounty}{addCity} ORDER BY [Code];";
		using AddressContextBase context = new();
		return context.Postcodes.FromSql( query ).Select( z => z.Code ).ToList();
	}

	#endregion
}