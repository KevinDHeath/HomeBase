using Microsoft.AspNetCore.Mvc;
using EFCore.Data.Models;

namespace EFCore.RestApi.Controllers;

/// <summary>Defines the available HTTP verbs for Movies.</summary>
[Route( @"api/movie" )]
[ApiController]
[Produces( "application/json" )]
public class MovieController : ControllerBase
{
	#region Constructor and Variables

	private readonly MovieService _service;

	/// <summary>Initializes a new instance of the MovieController class.</summary>
	/// <param name="service">A Movie service.</param>
	public MovieController( MovieService service )
	{
		_service = service;
	}

	#endregion

	/// <summary>Get a collection of Movies.</summary>
	/// <param name="count">Maximum count of results to return.</param>
	/// <param name="last">Last Id in previous page.</param>
	/// <returns>A result set of Movie details.</returns>
	[HttpGet]
	public async Task<ActionResult<ResultsSet<Movie>>> Get( [FromQuery] int? count, [FromQuery] int? last )
	{
		var uri = BaseService.GetRequestUri( ControllerContext.HttpContext.Request );
		return await _service.GetResultSet( uri, count, last );
	}

	/// <summary>Get a specific Movie.</summary>
	/// <param name="id">Movie Id.</param>
	/// <returns>The Movie details.</returns>
	/// <response code="404">Not found, nothing is returned.</response>
	[HttpGet( "{id}" )]
	public async Task<ActionResult<Movie>> Get( int id )
	{
		var result = await _service.Read( id );
		if( result is null ) { return NotFound( result ); }

		return Ok( result );
	}

	/// <summary>Create a Movie.</summary>
	/// <param name="details">Movie details.</param>
	/// <returns>The added Movie details.</returns>
	[HttpPost]
	public async Task<ActionResult<Movie?>> Post( Movie details )
	{
		var result = await _service.Create( details );
		return Ok( result );
	}

	/// <summary>Update a Movie.</summary>
	/// <param name="id">Movie Id.</param>
	/// <param name="changes">Movie changes.</param>
	/// <returns>The updated Movie details.</returns>
	/// <response code="404">Not found, nothing is returned.</response>
	[HttpPut( "{id}" )]
	public async Task<ActionResult<Movie?>> Put( int id, Movie changes )
	{
		var result = await _service.Update( id, changes );
		if( result is null ) { return NotFound( result ); }

		return Ok( result );
	}

	/// <summary>Delete a Movie.</summary>
	/// <param name="id">Movie Id.</param>
	/// <returns>The deleted Movie details.</returns>
	/// <response code="404">Not found, nothing is returned.</response>
	[HttpDelete( "{id}" )]
	public async Task<ActionResult<List<Movie>>> Delete( int id )
	{
		var result = await _service.Delete( id );
		if( result is null ) return NotFound( result );

		return Ok( result );
	}
}