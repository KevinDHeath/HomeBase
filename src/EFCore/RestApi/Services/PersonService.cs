using EFCore.Data;

namespace EFCore.RestApi.Services;

/// <summary>Defines the available database operations for People.</summary>
public class PersonService
{
	#region Constructor and Variables

	private readonly EFCoreDbContext _context;

	/// <summary>Initializes a new instance of the PersonService class.</summary>
	/// <param name="context">Entity database context.</param>
	public PersonService( EFCoreDbContext context )
	{
		_context = context;
	}

	#endregion

	/// <summary>Gets a collection of People.</summary>
	/// <param name="uri">Request Uri.</param>
	/// <param name="count">Maximum count of results to return.</param>
	/// <param name="last">Last Id in previous page.</param>
	/// <returns>A result set of Person details.</returns>
	public async Task<ResultsSet<Person>> GetResultSet( string uri, int? count, int? last )
	{
		count ??= 0;
		ResultsSet<Person> rtn = new( count.Value )
		{
			Total = await _context.People.CountAsync()
		};

		last ??= 0;
		rtn.Results = await _context.People.OrderBy( o => o.Id )
			.Where( w => w.Id > last )
			.Take( rtn.Max ).ToListAsync();

		BaseService.ApplyPaging( rtn, last.Value, uri );

		return rtn;
	}

	#region CRUD Operations

	/// <summary>Creates a Person.</summary>
	/// <param name="details">The Person details.</param>
	/// <returns>The created Person details.</returns>
	public async Task<Person> Create( Person details )
	{
		_ = _context.People.Add( details );
		_ = await _context.SaveChangesAsync();
		return details;
	}

	/// <summary>Gets a specific Person.</summary>
	/// <param name="id">Unique Person Id.</param>
	/// <returns>The Person details.</returns>
	public async Task<Person?> Read( int id )
	{
		return await _context.People.Include( x => x.Address ).FirstOrDefaultAsync( x => x.Id == id );
	}

	/// <summary>Updates a specific Person.</summary>
	/// <param name="id">Unique Person Id.</param>
	/// <param name="changes">The Person detail changes.</param>
	/// <returns>The updated Person details.</returns>
	public async Task<Person?> Update( int id, Person changes )
	{
		Person? current = await Read( id );
		if( current is null ) { return current; }

		current.Update( changes );
		_ = await _context.SaveChangesAsync();
		return current;
	}

	/// <summary>Deletes a Person.</summary>
	/// <param name="id">Unique Person Id.</param>
	/// <returns>The deleted Person details.</returns>
	public async Task<Person?> Delete( int id )
	{
		Person? current = await Read( id );
		if( current is null ) return null;

		_context.People.Remove( current );
		_ = await _context.SaveChangesAsync();
		return current;
	}

	#endregion
}