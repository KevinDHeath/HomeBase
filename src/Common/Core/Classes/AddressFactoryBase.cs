using Common.Core.Models;

namespace Common.Core.Classes;

/// <summary>Contains data used for Addresses.</summary>
public abstract class AddressFactoryBase
{
	/// <summary>Standard string comparison.</summary>
	protected static readonly StringComparison sCompare = StringComparison.OrdinalIgnoreCase;

	private static IList<Province>? _provinces;
	private static IList<CountryCode>? _countries;

	#region Public Properties

	/// <summary>Indicates whether to use Alpha-2 ISO-3166 Country codes.<br/>
	/// The default is to use Alpha-3 codes.</summary>
	/// <remarks>If required, true must be passed in the constructor of an AddressData object.</remarks>
	public static bool UseAlpha2 { get; protected set; }

	/// <summary>Gets the default ISO-3166 Country code.</summary>
	public static string DefaultCountry => UseAlpha2 ? "US" : "USA";

	/// <summary>Gets a sorted list of ISO-3166 Country data.</summary>
	public static IList<CountryCode> Countries
	{
		get => _countries is null ? new List<CountryCode>() : _countries;
		protected set => _countries = value;
	}

	/// <summary>Gets a list of Provinces.</summary>
	public static IList<Province> Provinces
	{
		get => _provinces is null ? new List<Province>() : _provinces;
		protected set => _provinces ??= value;
	}

	/// <summary>Gets a list of US Postcodes.</summary>
	public static IList<Postcode> Postcodes { get; set; } = new List<Postcode>();

	/// <summary>Gets the total number of Postcodes available.</summary>
	public static int PostcodeCount { get; protected set; }

	#endregion

	#region Protected Methods

	/// <summary>Sets the list of Countries.</summary>
	/// <param name="list">Collection of all ISO-3166 Country details.</param>
	protected static void SetCountries( IList<ISOCountry> list )
	{
		Countries = list.OrderBy( c => c.Name )
			.Select( c => new CountryCode( UseAlpha2 ? c.Alpha2 : c.Alpha3, c.Name ) ).ToList();
	}

	#endregion

	#region Public Methods

	/// <summary>Checks whether a Country code is valid.</summary>
	/// <param name="code">ISO-3166 Country code.</param>
	/// <returns>True if the Country code was found.</returns>
	public static bool CheckCountryCode( string? code )
	{
		if( code is null || code.Length != ( UseAlpha2 ? 2 : 3 ) ) { return false; }
		var country = Countries.FirstOrDefault( x => x.Code.Equals( code, sCompare ) );
		return country is not null;
	}

	/// <summary>Checks whether a Province code is valid.</summary>
	/// <param name="code">Province code.</param>
	/// <returns>True if the Province code was found.</returns>
	public static bool CheckProvinceCode( string? code )
	{
		if( code is null || code.Length > 10 ) { return false; }
		var state = Provinces.FirstOrDefault( x =>
		{
			if( x is null || x.Code is null ) { return false; }
			return x.Code.Equals( code, sCompare );
		} );
		return state is not null;
	}

	/// <summary>Gets the name for a Province code.</summary>
	/// <param name="code">Province code.</param>
	/// <returns>An empty string is returned if the Province code was not found.</returns>
	public static string GetProvinceName( string? code )
	{
		if( code is null || code.Length > 10 ) { return string.Empty; }
		var state = Provinces.FirstOrDefault( x =>
		{
			if( x is null || x.Code is null ) { return false; }
			return x.Code.Equals( code, sCompare );
		} );
		return state is not null ? state.Name : string.Empty;
	}

	/// <summary>Gets the information for a requested Postcode.</summary>
	/// <param name="code">Postal Service code.</param>
	/// <returns>Null is returned if the Postcode was not found.</returns>
	public static Postcode? GetPostcode( string? code )
	{
		if( code is null || ( DefaultCountry.StartsWith( "US" ) & code.Length < 5 ) ) { return null; }
		if( code.Length > 5 & DefaultCountry.StartsWith( "US" ) ) { code = code[..5]; }
		return Postcodes.FirstOrDefault( z => z.Code == code );
	}

	#endregion
}