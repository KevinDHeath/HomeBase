using Common.Data.SqlServer;

namespace EFCore.RestApi.Services;

/// <summary>Defines the available database operations for Postcodes.</summary>
/// <remarks>Initializes a new instance of the PostcodeService class.</remarks>
/// <param name="context">Entity database context.</param>
public class PostcodeService( FullContextBase context )
{
	private readonly FullContextBase _context = context;

	/// <summary>Gets the count of Postcodes.</summary>
	/// <param name="province">Optional Postal Service Province abbreviation.</param>
	/// <param name="county">Optional County name in the Province (if supplied).</param>
	/// <param name="city">Optional City name in the County or Province (if supplied).</param>
	/// <returns>The total number of Postcodes as a result set.</returns>
	/// <remarks>The result set can contain a collection of Postcodes if any of the
	/// optional parameters are supplied.</remarks>
	public async Task<ResultsSet<Postcode>> GetResultSet( string? province, string? county, string? city )
	{
		ResultsSet<Postcode> rtn = new( 1 )
		{
			Total = await _context.Postcodes.CountAsync()
		};

		#region Optional Parameters

		if( province is null ) { return rtn; }
		if( county is not null ) { county = Uri.UnescapeDataString( county ); }
		if( city is not null ) { city = Uri.UnescapeDataString( city ); }

		int? total = rtn.Total;
		rtn = new ResultsSet<Postcode>( total ) { Total = total };

		if( !string.IsNullOrWhiteSpace( county ) && !string.IsNullOrWhiteSpace( city ) )
		{
			// Province, County and City provided
			var results = from a in _context.Postcodes
							where a.Province == province && a.County == county && a.City == city
							select a;
			rtn.Results = results.ToList();
		}
		else if( !string.IsNullOrWhiteSpace( county ) )
		{
			// Province and County provided
			var results = from a in _context.Postcodes
							where a.Province == province && a.County == county
							select a;
			rtn.Results = results.ToList();
		}
		else if( !string.IsNullOrWhiteSpace( city ) )
		{
			// Province and City provided
			var results = from a in _context.Postcodes
							where a.Province == province && a.City == city
							select a;
			rtn.Results = results.ToList();
		}
		else
		{
			// Province only provided
			var results = from a in _context.Postcodes
							where a.Province == province
							select a;
			rtn.Results = results.ToList();
		}

		#endregion

		return rtn;
	}

	/// <summary>Gets a specific Postcode.</summary>
	/// <param name="code">The Postcode.</param>
	/// <returns>The Postcode details.</returns>
	public async Task<Postcode?> Read( string code )
	{
		if( code.Length > 5 ) { code = code[..5]; }

		var result = await _context.Postcodes.SingleOrDefaultAsync( s => s.Code == code );
		if( result is null ) return null;

		return result;
	}
}