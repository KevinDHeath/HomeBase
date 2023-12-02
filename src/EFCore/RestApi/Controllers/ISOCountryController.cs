using Microsoft.AspNetCore.Mvc;

namespace EFCore.RestApi.Controllers;

/// <summary>Defines the available HTTP verbs for ISO Countries.</summary>
[Route( @"api/isocountry" )]
[ApiController]
[Produces( "application/json" )]
public class ISOCountryController : ControllerBase
{
	#region Constructor and Variables

	private readonly ISOCountryService _service;

	/// <summary>Initializes a new instance of the ISOCountryController class.</summary>
	/// <param name="service">An ISO Country service.</param>
	public ISOCountryController( ISOCountryService service )
	{
		_service = service;
	}

	#endregion

	/// <summary>Get a collection of all ISO Countries.</summary>
	/// <returns>A result set of ISO Country details.</returns>
	[HttpGet]
	public async Task<ActionResult<ResultsSet<ISOCountry>>> Get()
	{
		return await _service.GetResultSet();
	}
}