using Microsoft.AspNetCore.Mvc;
using WarcraftGearPlanner.Server.Models;
using WarcraftGearPlanner.Server.Services;

namespace WarcraftGearPlanner.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ItemsController(IBattleNetService battleNetService) : ControllerBase
{
	private readonly IBattleNetService _battleNetService = battleNetService;

	[HttpGet("media/{itemId}")]
	[ProducesResponseType<MediaReference>(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<MediaReference>> GetItemMedia(int itemId)
	{
		var itemMedia = await _battleNetService.GetItemMedia(itemId);
		return itemMedia is null ? NotFound() : Ok(itemMedia);
	}

	[HttpGet("media")]
	[ProducesResponseType<IEnumerable<MediaReference>>(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<IEnumerable<MediaReference>>> GetItemMedia([FromQuery] int[] itemIds)
	{
		var itemMedia = await _battleNetService.GetItemMedia(itemIds);
		return itemMedia is null ? NotFound() : Ok(itemMedia);
	}
}
