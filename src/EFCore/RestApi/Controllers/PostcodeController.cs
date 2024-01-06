using Microsoft.AspNetCore.Mvc;

namespace EFCore.RestApi.Controllers;

/// <summary>Defines the available HTTP verbs for Postcodes.</summary>
/// <remarks>Initializes a new instance of the PostcodeController class.</remarks>
/// <param name="service">A Postcode service.</param>
[Route( @"api/postcode" )]
[ApiController]
[Produces( "application/json" )]
public class PostcodeController( PostcodeService service ) : ControllerBase
{
	private readonly PostcodeService _service = service;

	/// <summary>Get the count of Postcodes.</summary>
	/// <param name="province">Postal Service Province abbreviation.</param>
	/// <param name="county">A County name in the Province.</param>
	/// <param name="city">A City name in the County</param>
	/// <returns>A result set containing the total count.</returns>
	[HttpGet()]
	public async Task<ActionResult<ResultsSet<Postcode>>> Get(
		[FromQuery] string? province, [FromQuery] string? county, [FromQuery] string? city )
	{
		return await _service.GetResultSet( province, county, city );
	}

	/// <summary>Get a specific Postcode.</summary>
	/// <param name="postCode">Postal Service postcode.</param>
	/// <returns>The Postcode details.</returns>
	/// <response code="404">Not found, nothing is returned.</response>
	[HttpGet( "{postCode}" )]
	public async Task<ActionResult<Postcode>> Get( string postCode )
	{
		var result = await _service.Read( postCode );
		if( result is null ) { return NotFound( null ); }

		return Ok( result );
	}
}