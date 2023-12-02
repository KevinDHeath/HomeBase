using Microsoft.AspNetCore.Mvc;

namespace EFCore.RestApi.Controllers;

/// <summary>Defines the available HTTP verbs for Companies.</summary>
[Route( @"api/company" )]
[ApiController]
[Produces( "application/json" )]
public class CompanyController : ControllerBase
{
	#region Constructor and Variables

	private readonly CompanyService _service;

	/// <summary>Initializes a new instance of the CompanyController class.</summary>
	/// <param name="service">A company service.</param>
	public CompanyController( CompanyService service )
	{
		_service = service;
	}

	#endregion

	/// <summary>Get a collection of Companies.</summary>
	/// <param name="count">Maximum count of results to return.</param>
	/// <param name="last">Last Id in previous page.</param>
	/// <returns>A result set of Company details.</returns>
	[HttpGet]
	public async Task<ActionResult<ResultsSet<Company>>> Get( [FromQuery] int? count, [FromQuery] int? last )
	{
		var uri = BaseService.GetRequestUri( ControllerContext.HttpContext.Request );
		return await _service.GetResultSet( uri, count, last );
	}

	/// <summary>Get a specific Company.</summary>
	/// <param name="id">Company Id.</param>
	/// <returns>The Company details.</returns>
	/// <response code="404">Not found, nothing is returned.</response>
	[HttpGet( "{id}" )]
	public async Task<ActionResult<Company>> Get( int id )
	{
		var result = await _service.Read( id );
		if( result is null ) { return NotFound( null ); }

		return Ok( result );
	}

	/// <summary>Create a Company.</summary>
	/// <param name="details">Company details.</param>
	/// <returns>The added Company details.</returns>
	[HttpPost]
	public async Task<ActionResult<Company?>> Post( Company details )
	{
		var result = await _service.Create( details );
		return Ok( result );
	}

	/// <summary>Update a Company.</summary>
	/// <param name="id">Company Id.</param>
	/// <param name="changes">Company changes.</param>
	/// <returns>The updated Company details.</returns>
	/// <response code="404">Not found, nothing is returned.</response>
	[HttpPut( "{id}" )]
	public async Task<ActionResult<Company?>> Put( int id, Company changes )
	{
		var result = await _service.Update( id, changes );
		if( result is null ) { return NotFound( result ); }

		return Ok( result );
	}

	/// <summary>Delete a Company.</summary>
	/// <param name="id">Company Id.</param>
	/// <returns>The deleted Company details.</returns>
	/// <response code="404">Not found, nothing is returned.</response>
	[HttpDelete( "{id}" )]
	public async Task<ActionResult<List<Company>>> Delete( int id )
	{
		var result = await _service.Delete( id );
		if( result is null ) return NotFound( result );

		return Ok( result );
	}
}