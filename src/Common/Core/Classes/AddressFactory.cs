using Common.Core.Models;

namespace Common.Core.Classes;

/// <summary>Contains data used for Addresses.</summary>
public abstract class AddressFactory
{
	/// <summary>Standard string comparison.</summary>
	protected static readonly StringComparison sCompare = StringComparison.OrdinalIgnoreCase;

	private static IList<USState>? _usStates;
	private static IList<CountryCode>? _countries;

	#region Public Properties

	/// <summary>Indicates whether to use Alpha-2 ISO Country codes.<br/>
	/// The default is to use Alpha-3 codes.</summary>
	/// <remarks>If required, true must be passed in the constructor of an AddressData object.</remarks>
	public static bool UseAlpha2 { get; protected set; }

	/// <summary>Gets the default ISO Country code.</summary>
	public static string DefaultCountry => UseAlpha2 ? "US" : "USA";

	/// <summary>Gets a sorted list of ISO Country data.</summary>
	public static IList<CountryCode> Countries
	{
		get => _countries is null ? new List<CountryCode>() : _countries;
		protected set => _countries = value;
	}

	/// <summary>Gets a list of US State data.</summary>
	public static IList<USState> States
	{
		get => _usStates is null ? new List<USState>() : _usStates;
		protected set => _usStates ??= value;
	}

	/// <summary>Gets a list of US 5-digit Zip codes.</summary>
	public static IList<USZipCode> ZipCodes { get; set; } = new List<USZipCode>();

	/// <summary>Gets the total number of Zip Codes available.</summary>
	public static int ZipCodeCount { get; protected set; }

	#endregion

	#region Protected Methods

	/// <summary>Sets the list of Countries.</summary>
	/// <param name="list">Collection of all ISO Country details.</param>
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

	/// <summary>Checks whether a State code is valid.</summary>
	/// <param name="code">2-digit US Postal Service State abbreviation.</param>
	/// <returns>True if the State code was found.</returns>
	public static bool CheckStateCode( string? code )
	{
		if( code is null || code.Length != 2 ) { return false; }
		var state = States.FirstOrDefault( x =>
		{
			if( x is null || x.Alpha is null ) { return false; }
			return x.Alpha.Equals( code, sCompare );
		} );
		return state is not null;
	}

	/// <summary>Gets the name for a State code.</summary>
	/// <param name="code">2-digit US Postal Service State abbreviation.</param>
	/// <returns>An empty string is returned if the State code was not found.</returns>
	public static string GetStateName( string? code )
	{
		if( code is null || code.Length != 2 ) { return string.Empty; }
		var state = States.FirstOrDefault( x =>
		{
			if( x is null || x.Alpha is null ) { return false; }
			return x.Alpha.Equals( code, sCompare );
		} );
		return state is not null ? state.Name : string.Empty;
	}

	/// <summary>Gets the information for a requested US Zip code.</summary>
	/// <param name="usZipCode">5-digit US Postal Service Zip code.</param>
	/// <returns>Null is returned if the Zip code was not found.</returns>
	public static USZipCode? GetZipCode( string? usZipCode )
	{
		if( usZipCode is null || usZipCode.Length < 5 ) { return null; }
		if( usZipCode.Length > 5 ) { usZipCode = usZipCode[..5]; }
		return ZipCodes.FirstOrDefault( z => z.ZipCode == usZipCode );
	}

	#endregion
}