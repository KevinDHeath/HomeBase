using Microsoft.AspNetCore.Mvc;

namespace EFCore.RestApi.Controllers;

/// <summary>Defines the available HTTP verbs for People.</summary>
/// <remarks>Initializes a new instance of the PersonController class.</remarks>
/// <param name="service">A person service.</param>
[Route( @"api/person" )]
[ApiController]
[Produces( "application/json" )]
public class PersonController( PersonService service ) : ControllerBase
{
	private readonly PersonService _service = service;

	/// <summary>Get a collection of People.</summary>
	/// <param name="count">Maximum count of results to return.</param>
	/// <param name="last">Last Id in previous page.</param>
	/// <returns>A result set of Person details.</returns>
	[HttpGet]
	public async Task<ActionResult<ResultsSet<Person>>> Get( [FromQuery] int? count, [FromQuery] int? last )
	{
		var uri = BaseService.GetRequestUri( ControllerContext.HttpContext.Request );
		return await _service.GetResultSet( uri, count, last );
	}

	/// <summary>Get a specific Person.</summary>
	/// <param name="id">Person Id.</param>
	/// <returns>Person details.</returns>
	/// <response code="404">Not found, nothing is returned.</response>
	[HttpGet( "{id}" )]
	public async Task<ActionResult<Person>> Get( int id )
	{
		var result = await _service.Read( id );
		if( result is null ) { return NotFound( null ); }

		return Ok( result );
	}

	/// <summary>Create a Person.</summary>
	/// <param name="details">Person details.</param>
	/// <returns>The added Person details.</returns>
	[HttpPost]
	public async Task<ActionResult<Person?>> Post( Person details )
	{
		var result = await _service.Create( details );
		return Ok( result );
	}

	/// <summary>Update a Person.</summary>
	/// <param name="id">Person Id.</param>
	/// <param name="changes">Person changes.</param>
	/// <returns>The updated Person details.</returns>
	/// <response code="404">Not found, nothing is returned.</response>
	[HttpPut( "{id}" )]
	public async Task<ActionResult<Person?>> Put( int id, Person changes )
	{
		var result = await _service.Update( id, changes );
		if( result is null ) { return NotFound( result ); }

		return Ok( result );
	}

	/// <summary>Delete a Person.</summary>
	/// <param name="id">Person Id.</param>
	/// <returns>The deleted Person details.</returns>
	/// <response code="404">Not found, nothing is returned.</response>
	[HttpDelete( "{id}" )]
	public async Task<ActionResult<List<Person>>> Delete( int id )
	{
		var result = await _service.Delete( id );
		if( result is null ) return NotFound( result );

		return Ok( result );
	}
}