using EFCore.Data;

namespace EFCore.RestApi.Services;

/// <summary>Defines the available database operations for Companies.</summary>
public class CompanyService
{
	#region Constructor and Variables

	private readonly EFCoreDbContext _context;

	/// <summary>Initializes a new instance of the CompanyService class.</summary>
	/// <param name="context">Entity database context.</param>
	public CompanyService( EFCoreDbContext context )
	{
		_context = context;
	}

	#endregion

	/// <summary>Gets a collection of Companies.</summary>
	/// <param name="uri">Request Uri.</param>
	/// <param name="count">Maximum count of results to return.</param>
	/// <param name="last">Last Id in previous page.</param>
	/// <returns>A result set of Company details.</returns>
	public async Task<ResultsSet<Company>> GetResultSet( string uri, int? count, int? last )
	{
		count ??= 0;
		ResultsSet<Company> rtn = new( count.Value )
		{
			Total = await _context.Companies.CountAsync()
		};

		last ??= 0;
		rtn.Results = await _context.Companies.OrderBy( o => o.Id )
			.Where( w => w.Id > last )
			.Take( rtn.Max ).ToListAsync();

		BaseService.ApplyPaging( rtn, last.Value, uri );

		return rtn;
	}

	#region CRUD Operations

	/// <summary>Creates a Company.</summary>
	/// <param name="details">The Company details.</param>
	/// <returns>The created Company details.</returns>
	public async Task<Company> Create( Company details )
	{
		_context.Companies.Add( details );
		_ = await _context.SaveChangesAsync();
		return details;
	}

	/// <summary>Gets a specific Company.</summary>
	/// <param name="id">Unique Company Id.</param>
	/// <returns>The Company details.</returns>
	public async Task<Company?> Read( int id )
	{
		return await _context.Companies.Include( x => x.Address ).FirstOrDefaultAsync( x => x.Id == id );
	}

	/// <summary>Updates a specific Company.</summary>
	/// <param name="id">Unique Company Id.</param>
	/// <param name="changes">The Company detail changes.</param>
	/// <returns>The updated Company details.</returns>
	public async Task<Company?> Update( int id, Company changes )
	{
		Company? current = await Read( id );
		if( current is null ) { return current; }

		current.Update( changes );
		_ = await _context.SaveChangesAsync();
		return current;
	}

	/// <summary>Deletes a Company.</summary>
	/// <param name="id">Unique Company Id.</param>
	/// <returns>The deleted Company details.</returns>
	public async Task<Company?> Delete( int id )
	{
		Company? current = await Read( id );
		if( current is null ) return null;

		_context.Companies.Remove( current );
		_ = await _context.SaveChangesAsync();
		return current;
	}

	#endregion
}