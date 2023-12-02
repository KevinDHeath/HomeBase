using EFCore.Data;
using EFCore.Data.Models;

namespace EFCore.RestApi.Services;

/// <summary>Defines the available database operations for Super Heroes.</summary>
public class SuperHeroService
{
	#region Constructor and Variables

	private readonly EFCoreDbContext _context;

	/// <summary>Initializes a new instance of the SuperHeroService class.</summary>
	/// <param name="context">Entity database context.</param>
	public SuperHeroService( EFCoreDbContext context )
	{
		_context = context;
	}

	#endregion

	/// <summary>Gets a collection of Super Heroes.</summary>
	/// <param name="uri">Request Uri.</param>
	/// <param name="count">Maximum count of results to return.</param>
	/// <param name="last">Last Id in previous page.</param>
	/// <returns>A result set of Super Hero details.</returns>
	public async Task<ResultsSet<SuperHero>> GetResultSet( string uri, int? count, int? last )
	{
		count ??= 0;
		ResultsSet<SuperHero> rtn = new( count.Value )
		{
			Total = await _context.SuperHeroes.CountAsync()
		};

		last ??= 0;
		rtn.Results = await _context.SuperHeroes.OrderBy( o => o.Id )
			.Where( w => w.Id > last )
			.Take( rtn.Max ).ToListAsync();

		BaseService.ApplyPaging( rtn, last.Value, uri );

		return rtn;
	}

	#region CRUD Operations

	/// <summary>Creates a Super Hero.</summary>
	/// <param name="details">The Super Hero details.</param>
	/// <returns>The created Super Hero details.</returns>
	public async Task<SuperHero> Create( SuperHero details )
	{
		_ = _context.SuperHeroes.Add( details );
		_ = await _context.SaveChangesAsync();
		return details;
	}

	/// <summary>Gets a specific Super Hero.</summary>
	/// <param name="id">Unique Super Hero Id.</param>
	/// <returns>The Super Hero details.</returns>
	public async Task<SuperHero?> Read( int id )
	{
		return await _context.SuperHeroes.FindAsync( id );
	}

	/// <summary>Updates a specific Super Hero.</summary>
	/// <param name="id">Unique Super Hero Id.</param>
	/// <param name="changes">The Super Hero detail changes.</param>
	/// <returns>The updated Super Hero details.</returns>
	public async Task<SuperHero?> Update( int id, SuperHero changes )
	{
		SuperHero? current = await Read( id );
		if( current is null ) { return current; }

		if( current.FirstName != changes.FirstName ) { current.FirstName = changes.FirstName; }
		if( current.LastName != changes.LastName ) { current.LastName = changes.LastName; }
		if( current.Name != changes.Name ) { current.Name = changes.Name; }
		if( current.Place != changes.Place ) { current.Place = changes.Place; }
		if( current.Publisher != changes.Publisher ) { current.Publisher = changes.Publisher; }
		_ = await _context.SaveChangesAsync();
		return current;
	}

	/// <summary>Deletes a Super Hero.</summary>
	/// <param name="id">Unique Super Hero Id.</param>
	/// <returns>The deleted Super Hero details.</returns>
	public async Task<SuperHero?> Delete( int id )
	{
		SuperHero? current = await Read( id );
		if( current is null ) return null;

		_context.SuperHeroes.Remove( current );
		_ = await _context.SaveChangesAsync();
		return current;
	}

	#endregion
}