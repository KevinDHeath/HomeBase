using EFCore.Data;

namespace EFCore.RestApi.Services;

/// <summary>Defines the available database operations for ISO Countries.</summary>
public class ISOCountryService
{
	#region Constructor and Variables

	private readonly EFCoreDbContext _context;

	/// <summary>Initializes a new instance of the ISOCountryService class.</summary>
	/// <param name="context">Entity database context.</param>
	public ISOCountryService( EFCoreDbContext context )
	{
		_context = context;
	}

	#endregion

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