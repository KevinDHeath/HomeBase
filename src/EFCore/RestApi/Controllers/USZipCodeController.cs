using Microsoft.AspNetCore.Mvc;

namespace EFCore.RestApi.Controllers;

/// <summary>Defines the available HTTP verbs for US Zip codes.</summary>
[Route( @"api/uszipcode" )]
[ApiController]
[Produces( "application/json" )]
public class USZipCodeController : ControllerBase
{
	#region Constructor and Variables

	private readonly USZipCodeService _service;

	/// <summary>Initializes a new instance of the USZipCodeController class.</summary>
	/// <param name="service">A US Zip Code service.</param>
	public USZipCodeController( USZipCodeService service )
	{
		_service = service;
	}

	#endregion

	/// <summary>Get the count of US Zip codes.</summary>
	/// <param name="state">A 2-digit US Postal Service State abbreviation.</param>
	/// <param name="county">A County name in the State </param>
	/// <param name="city">A City name in the County</param>
	/// <returns>A result set containing the total count.</returns>
	[HttpGet()]
	public async Task<ActionResult<ResultsSet<USZipCode>>> Get(
		[FromQuery] string? state, [FromQuery] string? county, [FromQuery] string? city )
	{
		return await _service.GetResultSet( state, county, city );
	}

	/// <summary>Get a specific US Zip code.</summary>
	/// <param name="zipCode">US Postal Service Zip code.</param>
	/// <returns>The Zip code details.</returns>
	/// <response code="404">Not found, nothing is returned.</response>
	[HttpGet( "{zipCode}" )]
	public async Task<ActionResult<USZipCode>> Get( string zipCode )
	{
		var result = await _service.Read( zipCode );
		if( result is null ) { return NotFound( null ); }

		return Ok( result );
	}
}