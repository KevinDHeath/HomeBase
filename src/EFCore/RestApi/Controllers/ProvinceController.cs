using Microsoft.AspNetCore.Mvc;

namespace EFCore.RestApi.Controllers;

/// <summary>Defines the available HTTP verbs for Provinces.</summary>
/// <remarks>Initializes a new instance of the ProvinceController class.</remarks>
/// <param name="service">A Province service.</param>
[Route( @"api/province" )]
[ApiController]
[Produces( "application/json" )]
public class ProvinceController( ProvinceService service ) : ControllerBase
{
	private readonly ProvinceService _service = service;

	/// <summary>Get a collection of all Provinces.</summary>
	/// <returns>A result set of Province details.</returns>
	[HttpGet]
	public async Task<ActionResult<ResultsSet<Province>>> Get()
	{
		return await _service.GetResultSet();
	}
}