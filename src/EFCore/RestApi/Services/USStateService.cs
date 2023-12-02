using EFCore.Data;

namespace EFCore.RestApi.Services;

/// <summary>Defines the available database operations for US States.</summary>
public class USStateService
{
	#region Constructor and Variables

	private readonly EFCoreDbContext _context;

	/// <summary>Initializes a new instance of the USStateService class.</summary>
	/// <param name="context">Entity database context.</param>
	public USStateService( EFCoreDbContext context )
	{
		_context = context;
	}

	#endregion

	/// <summary>Gets a collection of all US States.</summary>
	/// <returns>A result set of US State details.</returns>
	public async Task<ResultsSet<USState>> GetResultSet()
	{
		int count = await _context.USStates.CountAsync();
		ResultsSet<USState> rtn = new( count )
		{
			Total = count,
			Results = await _context.USStates.ToListAsync()
		};

		return rtn;
	}
}