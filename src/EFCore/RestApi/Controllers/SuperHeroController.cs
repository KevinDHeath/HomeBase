using Microsoft.AspNetCore.Mvc;
using EFCore.Data.Models;

namespace EFCore.RestApi.Controllers;

/// <summary>Defines the available HTTP verbs for Super Heroes.</summary>
[Route( @"api/superhero" )]
[ApiController]
[Produces( "application/json" )]
public class SuperHeroController : ControllerBase
{
	#region Constructor and Variables

	private readonly SuperHeroService _service;

	/// <summary>Initializes a new instance of the SuperHeroController class.</summary>
	/// <param name="service">A Super Hero service.</param>
	public SuperHeroController( SuperHeroService service )
	{
		_service = service;
	}

	#endregion

	/// <summary>Get a collection of Super Heroes.</summary>
	/// <param name="count">Maximum count of results to return.</param>
	/// <param name="last">Last Id in previous page.</param>
	/// <returns>A result set of SuperHero details.</returns>
	[HttpGet]
	public async Task<ActionResult<ResultsSet<SuperHero>>> Get( [FromQuery] int? count, [FromQuery] int? last )
	{
		var uri = BaseService.GetRequestUri( ControllerContext.HttpContext.Request );
		return await _service.GetResultSet( uri, count, last );
	}

	/// <summary>Get a specific Super Hero.</summary>
	/// <param name="id">Super Hero Id.</param>
	/// <returns>Super Hero details.</returns>
	/// <response code="404">Not found, nothing is returned.</response>
	[HttpGet( "{id}" )]
	public async Task<ActionResult<SuperHero>> Get( int id )
	{
		var result = await _service.Read( id );
		if( result is null ) { return NotFound( result ); }

		return Ok( result );
	}

	/// <summary>Create a Super Hero.</summary>
	/// <param name="details">Super Hero details.</param>
	/// <returns>The added Super Hero details.</returns>
	[HttpPost]
	public async Task<ActionResult<SuperHero?>> Post( SuperHero details )
	{
		var result = await _service.Create( details );
		return Ok( result );
	}

	/// <summary>Update a Super Hero.</summary>
	/// <param name="id">Super Hero Id.</param>
	/// <param name="changes">Super Hero changes.</param>
	/// <returns>The updated Super Hero details.</returns>
	/// <response code="404">Not found, nothing is returned.</response>
	[HttpPut( "{id}" )]
	public async Task<ActionResult<SuperHero?>> Put( int id, SuperHero changes )
	{
		var result = await _service.Update( id, changes );
		if( result is null ) { return NotFound( result ); }

		return Ok( result );
	}

	/// <summary>Delete a Super Hero.</summary>
	/// <param name="id">Super Hero Id.</param>
	/// <returns>The deleted Super Hero details.</returns>
	/// <response code="404">Not found, nothing is returned.</response>
	[HttpDelete( "{id}" )]
	public async Task<ActionResult<List<SuperHero>>> Delete( int id )
	{
		var result = await _service.Delete( id );
		if( result is null ) return NotFound( result );

		return Ok( result );
	}
}