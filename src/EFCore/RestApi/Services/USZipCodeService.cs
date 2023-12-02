using EFCore.Data;

namespace EFCore.RestApi.Services;

/// <summary>Defines the available database operations for US Zip codes.</summary>
public class USZipCodeService
{
	#region Constructor and Variables

	private readonly EFCoreDbContext _context;

	/// <summary>Initializes a new instance of the USZipCodeService class.</summary>
	/// <param name="context">Entity database context.</param>
	public USZipCodeService( EFCoreDbContext context )
	{
		_context = context;
	}

	#endregion

	/// <summary>Gets the count of US Zip codes.</summary>
	/// <param name="state">Optional 2-digit US Postal Service State abbreviation.</param>
	/// <param name="county">Optional County name in the State (if supplied).</param>
	/// <param name="city">Optional City name in the County or State (if supplied).</param>
	/// <returns>The total number of US Zip codes as a result set.</returns>
	/// <remarks>The result set can contain a collection of US Zip codes if any of the
	/// optional parameters are supplied.</remarks>
	public async Task<ResultsSet<USZipCode>> GetResultSet( string? state, string? county, string? city )
	{
		ResultsSet<USZipCode> rtn = new( 1 )
		{
			Total = await _context.USZipCodes.CountAsync()
		};

		#region Optional Parameters

		if( state is null || state.Length != 2 ) { return rtn; }
		if( county is not null ) { county = Uri.UnescapeDataString( county ); }
		if( city is not null ) { city = Uri.UnescapeDataString( city ); }

		int? total = rtn.Total;
		rtn = new ResultsSet<USZipCode>( total ) { Total = total };

		if( !string.IsNullOrWhiteSpace( county ) && !string.IsNullOrWhiteSpace( city ) )
		{
			// State, County and City provided
			var results = from a in _context.USZipCodes
							where a.State == state && a.County == county && a.City == city
							select a;
			rtn.Results = results.ToList();
		}
		else if( !string.IsNullOrWhiteSpace( county ) )
		{
			// State and County provided
			var results = from a in _context.USZipCodes
							where a.State == state && a.County == county
							select a;
			rtn.Results = results.ToList();
		}
		else if( !string.IsNullOrWhiteSpace( city ) )
		{
			// State and City provided
			var results = from a in _context.USZipCodes
							where a.State == state && a.City == city
							select a;
			rtn.Results = results.ToList();
		}
		else
		{
			// State only provided
			var results = from a in _context.USZipCodes
							where a.State == state
							select a;
			rtn.Results = results.ToList();
		}

		#endregion

		return rtn;
	}

	/// <summary>Gets a specific US Zip code.</summary>
	/// <param name="code">The US Zip code.</param>
	/// <returns>The US Zip code details.</returns>
	public async Task<USZipCode?> Read( string code )
	{
		if( code.Length > 5 ) { code = code[..5]; }

		var result = await _context.USZipCodes.SingleOrDefaultAsync( s => s.ZipCode == code );
		if( result is null ) return null;

		return result;
	}
}