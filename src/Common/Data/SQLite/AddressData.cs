﻿using System.ComponentModel;
using Common.Core.Classes;
using Common.Core.Models;

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
	/// <param name="isoCountry">The ISO-3166 Country code to use for Address data. The default is USA.</param>
	public AddressData( bool useAlpha2 = false, bool countries = true, bool provinces = true,
		bool postcodes = true, string isoCountry = "" )
	{
		using AddressContextBase context = new();
		if( countries & Countries.Count == 0 )
		{
			LoadCountries( useAlpha2, context );
			DefaultCountry = isoCountry;
		}
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
	/// <param name="province">Postal Service Province abbreviation.</param>
	/// <returns>An empty list is returned if the Province code was not found.</returns>
	[EditorBrowsable( EditorBrowsableState.Never )]
	public static IList<string?> GetCountyNames( string? province )
	{
		if( province is null || string.IsNullOrWhiteSpace( province ) ) { return new List<string?>(); }
		province = province.Trim();

		using AddressContextBase context = new();
		IQueryable<Postcode> codes = context.Postcodes.Where( z => z.Province.Equals( province ) );
		IQueryable<IGrouping<string?, Postcode>> names = codes.GroupBy( z => z.County ).OrderBy( k => k.Key );
		return names.Select( z => z.Key ).ToList();
	}

	/// <summary>Gets a sorted list of City names for a requested Province and County.</summary>
	/// <param name="province">Postal Service Province abbreviation.</param>
	/// <param name="county">County name.</param>
	/// <returns>An empty list is returned if the Province code or County name was not found.</returns>
	[EditorBrowsable( EditorBrowsableState.Never )]
	public static IList<string?> GetCityNames( string? province, string? county = null )
	{
		if( province is null || string.IsNullOrWhiteSpace( province ) ) { return new List<string?>(); }
		province = province.Trim();

		using AddressContextBase context = new();
		IQueryable<Postcode> pcs = context.Postcodes.Where( z => z.Province.Equals( province ) );
		if( county is not null ) { pcs = pcs.Where( z => z.County != null && z.County.Equals( county ) ); }

		IQueryable<IGrouping<string?, Postcode>> list = pcs.GroupBy( z => z.City ).OrderBy( k => k.Key );
		return list.Select( z => z.Key ).ToList();
	}

	/// <summary>Gets a sorted list of Postal codes for a requested Province, County and City.</summary>
	/// <param name="province">Postal Service Province abbreviation.</param>
	/// <param name="county">County name.</param>
	/// <param name="city">City name.</param>
	/// <returns>An empty list is returned if the Province code, County name, or City name was not found.</returns>
	[EditorBrowsable( EditorBrowsableState.Never )]
	public static IList<string?> GetPostcodes( string? province, string? county = null, string? city = null )
	{
		if( province is null || string.IsNullOrWhiteSpace( province ) ) { return new List<string?>(); }
		province = province.Trim();

		using AddressContextBase context = new();
		IQueryable<Postcode> pcs = context.Postcodes.Where( z => z.Province.Equals( province ) );
		if( county is not null ) { pcs = pcs.Where( z => z.County != null && z.County.Equals( county ) ); }
		if( city is not null ) { pcs = pcs.Where( z => z.City != null && z.City.Equals( city ) ); }

		IQueryable<IGrouping<string?, Postcode>> list = pcs.GroupBy( z => z.Code ).OrderBy( k => k.Key );
		return list.Select( z => z.Key ).ToList();
	}

	#endregion
}