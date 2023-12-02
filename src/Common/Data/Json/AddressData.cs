using System.ComponentModel;
using Common.Core.Classes;
using Common.Core.Models;

namespace Common.Data.Json;

/// <summary>Contains data used for Addresses.</summary>
public class AddressData : AddressFactory
{
	#region Constructor

	/// <summary>Initializes a new instance of the AddressData class.</summary>
	/// <param name="configFile">The name of the configuration file. This is not used for Json data.</param>
	/// <param name="useAlpha2">Indicates whether to use Alpha-2 ISO Country codes. The default is false.</param>
	/// <param name="countries">Indicates whether ISO Countries should be loaded. The default is true.</param>
	/// <param name="usStates">Indicates whether US States should be loaded. The default is true.</param>
	/// <param name="usZipCodes">Indicates whether US Zip Codes should be loaded. The default is true.</param>
#pragma warning disable IDE0060 // Remove unused parameter
	public AddressData( string configFile = "",
#pragma warning restore IDE0060 // Remove unused parameter
		bool useAlpha2 = false, bool countries = true, bool usStates = true, bool usZipCodes = true )
	{
		string? json;
		if( countries & Countries.Count == 0 )
		{
			if( useAlpha2 ) { UseAlpha2 = useAlpha2; }
			json = Factory.GetEmbeddedResource( "ISOCountries.json" );
			SetCountries( JsonHelper.DeserializeList<ISOCountry>( ref json ) );
		}
		if( usStates )
		{
			json = Factory.GetEmbeddedResource( "USStates.json" );
			States = JsonHelper.DeserializeList<USState>( ref json );
		}

		if( usZipCodes )
		{
			json = Factory.GetEmbeddedResource( "USZipCodes.json" );
			ZipCodes = JsonHelper.DeserializeList<USZipCode>( ref json );
			ZipCodeCount = ZipCodes.Count;
		}
	}

	#endregion

	#region Testing Methods

	/// <summary>Gets a sorted list of County names for a requested US State.</summary>
	/// <param name="code">2-digit US Postal Service State abbreviation.</param>
	/// <returns>An empty list is returned if the State code was not found.</returns>
	[EditorBrowsable( EditorBrowsableState.Never )]
	public static IList<string> GetCountyNames( string? code )
	{
		if( code is null || code.Length != 2 ) { return new List<string>(); }

		return ZipCodes.Where( z => z.State.Equals( code, sCompare ) )
			.OrderBy( z => z.County )
			.GroupBy( z => z.County )
			.Select( z => z.Key ).ToList();
	}

	/// <summary>Gets a sorted list of City names for a requested US State and County.</summary>
	/// <param name="state">2-digit US Postal Service State abbreviation.</param>
	/// <param name="county">County name.</param>
	/// <returns>An empty list is returned if the State code or County name was not found.</returns>
	[EditorBrowsable( EditorBrowsableState.Never )]
	public static IList<string> GetCityNames( string? state, string? county = null )
	{
		if( state is null || state.Length != 2 ) { return new List<string>(); }
		state = state.ToUpper() ?? string.Empty;

		if( !string.IsNullOrWhiteSpace( county ) )
		{
			return ZipCodes.Where( z => z.State == state && z.County.Equals( county, sCompare ) )
				.OrderBy( z => z.City )
				.GroupBy( z => z.City )
				.Select( z => z.Key ).ToList();
		}
		else
		{
			return ZipCodes.Where( z => z.State == state )
				.OrderBy( z => z.City )
				.GroupBy( z => z.City )
				.Select( z => z.Key ).ToList();
		}
	}

	/// <summary>Gets a sorted list of Zip codes for a requested US State, County and City.</summary>
	/// <param name="state">2-digit US Postal Service State abbreviation.</param>
	/// <param name="county">County name.</param>
	/// <param name="city">City name.</param>
	/// <returns>An empty list is returned if the State code, County name, or City name was not found.</returns>
	[EditorBrowsable( EditorBrowsableState.Never )]
	public static IList<string> GetZipCodes( string? state, string? county = null, string? city = null )
	{
		if( state is null || state.Length != 2 ) { return new List<string>(); }
		state = state.ToUpper() ?? string.Empty;

		if( !string.IsNullOrWhiteSpace( county ) && !string.IsNullOrWhiteSpace( city ) )
		{
			// County and City supplied
			return ZipCodes.Where( z => z.State == state && z.County.Equals( county, sCompare ) &&
				z.City.Equals( city, sCompare ) )
				.OrderBy( z => z.ZipCode )
				.Select( z => z.ZipCode ).ToList();
		}
		else if( !string.IsNullOrWhiteSpace( county ) && string.IsNullOrWhiteSpace( city ) )
		{
			// County but no City supplied
			return ZipCodes.Where( z => z.State == state && z.County.Equals( county, sCompare ) )
				.OrderBy( z => z.ZipCode )
				.Select( z => z.ZipCode ).ToList();
		}
		else if( string.IsNullOrWhiteSpace( county ) && !string.IsNullOrWhiteSpace( city ) )
		{
			// City but no County supplied
			return ZipCodes.Where( z => z.State == state && z.City.Equals( city, sCompare ) )
				.OrderBy( z => z.ZipCode )
				.Select( z => z.ZipCode ).ToList();
		}
		else
		{
			// No County or City supplied
			return ZipCodes.Where( z => z.State == state )
				.OrderBy( z => z.ZipCode )
				.Select( z => z.ZipCode ).ToList();
		}
	}

	/// <summary>Testing method to get a grouped collection of Zip codes.</summary>
	/// <returns>Collection of US Zip Codes grouped by US State.</returns>
	[EditorBrowsable( EditorBrowsableState.Never )]
	public static List<IGrouping<string, USZipCode>> GetZipCodes()
	{
		return ZipCodes.OrderBy( s => s.State ).GroupBy( s => s.State ).ToList();
	}

	#endregion
}