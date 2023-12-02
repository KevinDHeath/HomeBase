using Microsoft.AspNetCore.Mvc;

namespace EFCore.RestApi.Controllers;

/// <summary>Defines the available HTTP verbs for US States.</summary>
[Route( @"api/usstate" )]
[ApiController]
[Produces( "application/json" )]
public class USStateController : ControllerBase
{
	#region Constructor and Variables

	private readonly USStateService _service;

	/// <summary>Initializes a new instance of the USStateController class.</summary>
	/// <param name="service">A US State service.</param>
	public USStateController( USStateService service )
	{
		_service = service;
	}

	#endregion

	/// <summary>Get a collection of all US States.</summary>
	/// <returns>A result set of US States details.</returns>
	[HttpGet]
	public async Task<ActionResult<ResultsSet<USState>>> Get()
	{
		return await _service.GetResultSet();
	}
}