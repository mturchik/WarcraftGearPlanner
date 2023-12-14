using Microsoft.AspNetCore.Mvc;

namespace WarcraftGearPlanner.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class StatusController : ControllerBase
{
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<string> GetStatus()
	{
		var version = GetType().Assembly.GetName().Version;
		return Ok($"APPLICATION RUNNING - v{version}");
	}
}
