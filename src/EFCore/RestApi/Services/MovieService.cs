using Common.Data.SqlServer;
using Common.Data.SqlServer.Models;

namespace EFCore.RestApi.Services;

/// <summary>Defines the available database operations for Movies.</summary>
/// <remarks>Initializes a new instance of the MovieService class.</remarks>
/// <param name="context">Entity database context.</param>
public class MovieService( FullContextBase context )
{
	private readonly FullContextBase _context = context;

	/// <summary>Gets the collection of Movies.</summary>
	/// <param name="uri">Request Uri.</param>
	/// <param name="count">Maximum count of results to return.</param>
	/// <param name="last">Last Id in previous page.</param>
	/// <returns>A result set of Movie details.</returns>
	public async Task<ResultsSet<Movie>> GetResultSet( string uri, int? count, int? last )
	{
		count ??= 0;
		ResultsSet<Movie> rtn = new( count.Value )
		{
			Total = await _context.Movies.CountAsync()
		};

		last ??= 0;
		rtn.Results = await _context.Movies.OrderBy( o => o.Id )
			.Where( w => w.Id > last )
			.Take( rtn.Max ).ToListAsync();

		BaseService.ApplyPaging( rtn, last.Value, uri );

		return rtn;
	}

	#region CRUD Operations

	/// <summary>Creates a Movie.</summary>
	/// <param name="details">The Movie details.</param>
	/// <returns>The created Movie details.</returns>
	public async Task<Movie> Create( Movie details )
	{
		_context.Movies.Add( details );
		_ = await _context.SaveChangesAsync();
		return details;
	}

	/// <summary>Gets a specific Movie.</summary>
	/// <param name="id">Unique Movie Id.</param>
	/// <returns>The Movie details.</returns>
	public async Task<Movie?> Read( int id )
	{
		return await _context.Movies.FindAsync( id );
	}

	/// <summary>Updates a specific Movie.</summary>
	/// <param name="id">Unique Movie Id.</param>
	/// <param name="changes">The Movie detail changes.</param>
	/// <returns>The updated Movie details.</returns>
	public async Task<Movie?> Update( int id, Movie changes )
	{
		var current = await Read( id );
		if( current is null ) { return current; }

		if( current.Title != changes.Title ) { current.Title = changes.Title; }
		if( current.ReleaseYear != changes.ReleaseYear ) { current.ReleaseYear = changes.ReleaseYear; }
		if( current.Genre != changes.Genre ) { current.Genre = changes.Genre; }
		if( current.Duration != changes.Duration ) { current.Duration = changes.Duration; }
		_ = await _context.SaveChangesAsync();
		return current;
	}

	/// <summary>Deletes a Movie.</summary>
	/// <param name="id">Unique Movie Id.</param>
	/// <returns>The deleted Movie details.</returns>
	public async Task<Movie?> Delete( int id )
	{
		var current = await Read( id );
		if( current is null ) return null;

		_context.Movies.Remove( current );
		_ = await _context.SaveChangesAsync();
		return current;
	}

	#endregion
}