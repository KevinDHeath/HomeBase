using Common.Data.SqlServer;

namespace EFCore.RestApi.Services;

/// <summary>Defines the available database operations for ISO Countries.</summary>
/// <remarks>Initializes a new instance of the ISOCountryService class.</remarks>
/// <param name="context">Entity database context.</param>
public class ISOCountryService( FullContextBase context )
{
	private readonly FullContextBase _context = context;

	/// <summary>Gets a collection of all ISO Countries.</summary>
	/// <returns>A result set of ISO Country details.</returns>
	public async Task<ResultsSet<ISOCountry>> GetResultSet()
	{
		int count = await _context.ISOCountries.CountAsync();
		ResultsSet<ISOCountry> rtn = new( count )
		{
			Total = count,
			Results = await _context.ISOCountries.ToListAsync()
		};

		return rtn;
	}
}