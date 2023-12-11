using Microsoft.AspNetCore.Mvc;
using WarcraftGearPlanner.Models.Response;
using WarcraftGearPlanner.Services;

namespace WarcraftGearPlanner.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RealmsController(IBattleNetService battleNetService) : ControllerBase
{
	private readonly IBattleNetService _battleNetService = battleNetService;

	[HttpGet]
	[ProducesResponseType<RealmIndexResponse>(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<RealmIndexResponse>> GetRealms()
	{
		var realms = await _battleNetService.GetRealms();
		return realms is null ? NotFound() : Ok(realms);
	}
}
