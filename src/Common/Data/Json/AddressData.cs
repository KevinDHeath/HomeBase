using System.ComponentModel;
using Common.Core.Classes;
using Common.Core.Models;

namespace Common.Data.Json;

/// <summary>Contains data used for Addresses.</summary>
public class AddressData : AddressFactoryBase
{
	#region Constructor

	/// <summary>Initializes a new instance of the AddressData class.</summary>
	/// <param name="configFile">The name of the configuration file. This is not used for Json data.</param>
	/// <param name="useAlpha2">Indicates whether to use Alpha-2 ISO Country codes. The default is false.</param>
	/// <param name="countries">Indicates whether ISO Countries should be loaded. The default is true.</param>
	/// <param name="provinces">Indicates whether Provinces should be loaded. The default is true.</param>
	/// <param name="postcodes">Indicates whether Postcodes should be loaded. The default is true.</param>
#pragma warning disable IDE0060 // Remove unused parameter
	public AddressData( string configFile = "",
		bool useAlpha2 = false, bool countries = true, bool provinces = true, bool postcodes = true )
#pragma warning restore IDE0060 // Remove unused parameter
	{
		string? json;
		if( countries & Countries.Count == 0 )
		{
			if( useAlpha2 ) { UseAlpha2 = useAlpha2; }
			json = Factory.GetEmbeddedResource( "ISOCountries.json" );
			SetCountries( JsonHelper.DeserializeList<ISOCountry>( ref json ) );
		}
		if( provinces )
		{
			json = Factory.GetEmbeddedResource( "USProvinces.json" );
			Provinces = JsonHelper.DeserializeList<Province>( ref json );
		}

		if( postcodes )
		{
			json = Factory.GetEmbeddedResource( "USPostcodes.json" );
			Postcodes = JsonHelper.DeserializeList<Postcode>( ref json );
			PostcodeCount = Postcodes.Count;
		}
	}

	#endregion

	#region Testing Methods

	/// <summary>Gets a sorted list of County names for a requested Province.</summary>
	/// <param name="province">Postal Service Province abbreviation.</param>
	/// <returns>An empty list is returned if the Province code was not found.</returns>
	[EditorBrowsable( EditorBrowsableState.Never )]
	public static IList<string?> GetCountyNames( string? province )
	{
		if( province is null || string.IsNullOrWhiteSpace( province ) ) { return new List<string?>(); }
		province = province.Trim();

		return Postcodes.Where( z => z.Province.Equals( province, sCompare ) )
			.OrderBy( z => z.County )
			.GroupBy( z => z.County )
			.Select( z => z.Key ).ToList();
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

		if( !string.IsNullOrWhiteSpace( county ) )
		{
			return Postcodes.Where( z => z.Province == province && z.County is not null && z.County.Equals( county, sCompare ) )
				.OrderBy( z => z.City )
				.GroupBy( z => z.City )
				.Select( z => z.Key ).ToList();
		}
		else
		{
			return Postcodes.Where( z => z.Province == province )
				.OrderBy( z => z.City )
				.GroupBy( z => z.City )
				.Select( z => z.Key ).ToList();
		}
	}

	/// <summary>Gets a sorted list of Zip codes for a requested Province, County and City.</summary>
	/// <param name="province">Postal Service Province abbreviation.</param>
	/// <param name="county">County name.</param>
	/// <param name="city">City name.</param>
	/// <returns>An empty list is returned if the Province code, County name, or City name was not found.</returns>
	[EditorBrowsable( EditorBrowsableState.Never )]
	public static IList<string?> GetPostcodes( string? province, string? county = null, string? city = null )
	{
		if( province is null || string.IsNullOrWhiteSpace( province ) ) { return new List<string?>(); }
		province = province.Trim();

		if( !string.IsNullOrWhiteSpace( county ) && !string.IsNullOrWhiteSpace( city ) )
		{
			// County and City supplied
			return Postcodes.Where( z => z.Province == province && z.County is not null && z.County.Equals( county, sCompare ) &&
				z.City is not null && z.City.Equals( city, sCompare ) )
				.OrderBy( z => z.Code )
				.Select( z => z.Code ).ToList()!;
		}
		else if( !string.IsNullOrWhiteSpace( county ) && string.IsNullOrWhiteSpace( city ) )
		{
			// County but no City supplied
			return Postcodes.Where( z => z.Province == province && z.County is not null && z.County.Equals( county, sCompare ) )
				.OrderBy( z => z.Code )
				.Select( z => z.Code ).ToList()!;
		}
		else if( string.IsNullOrWhiteSpace( county ) && !string.IsNullOrWhiteSpace( city ) )
		{
			// City but no County supplied
			return Postcodes.Where( z => z.Province == province && z.City is not null && z.City.Equals( city, sCompare ) )
				.OrderBy( z => z.Code )
				.Select( z => z.Code ).ToList()!;
		}
		else
		{
			// No County or City supplied
			return Postcodes.Where( z => z.Province == province )
				.OrderBy( z => z.Code )
				.Select( z => z.Code ).ToList()!;
		}
	}

	/// <summary>Testing method to get a grouped collection of Postal codes.</summary>
	/// <returns>Collection of Postcodes grouped by Province.</returns>
	[EditorBrowsable( EditorBrowsableState.Never )]
	public static List<IGrouping<string, Postcode>> GetPostcodes()
	{
		return Postcodes.OrderBy( s => s.Province ).GroupBy( s => s.Province ).ToList();
	}

	#endregion
}