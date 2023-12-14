using Microsoft.AspNetCore.Mvc;
using WarcraftGearPlanner.Server.Models.Response;
using WarcraftGearPlanner.Server.Services;

namespace WarcraftGearPlanner.Server.Controllers;
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
		var realmIndex = await _battleNetService.GetRealmIndex();
		return realmIndex is null ? NotFound() : Ok(realmIndex.Realms);
	}
}
