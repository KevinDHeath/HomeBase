using Common.Data.SqlServer;

namespace EFCore.RestApi.Services;

/// <summary>Defines the available database operations for Provinces.</summary>
/// <remarks>Initializes a new instance of the ProvinceService class.</remarks>
/// <param name="context">Entity database context.</param>
public class ProvinceService( FullContextBase context )
{
	private readonly FullContextBase _context = context;

	/// <summary>Gets a collection of all Provinces.</summary>
	/// <returns>A result set of Province details.</returns>
	public async Task<ResultsSet<Province>> GetResultSet()
	{
		int count = await _context.Provinces.CountAsync();
		ResultsSet<Province> rtn = new( count )
		{
			Total = count,
			Results = await _context.Provinces.ToListAsync()
		};

		return rtn;
	}
}