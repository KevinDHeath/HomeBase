using Microsoft.AspNetCore.Mvc;

namespace EFCore.RestApi.Controllers;

/// <summary>Defines the available HTTP verbs for ISO Countries.</summary>
/// <remarks>Initializes a new instance of the ISOCountryController class.</remarks>
/// <param name="service">An ISO Country service.</param>
[Route( @"api/isocountry" )]
[ApiController]
[Produces( "application/json" )]
public class ISOCountryController( ISOCountryService service ) : ControllerBase
{
	private readonly ISOCountryService _service = service;

	/// <summary>Get a collection of all ISO Countries.</summary>
	/// <returns>A result set of ISO Country details.</returns>
	[HttpGet]
	public async Task<ActionResult<ResultsSet<ISOCountry>>> Get()
	{
		return await _service.GetResultSet();
	}
}