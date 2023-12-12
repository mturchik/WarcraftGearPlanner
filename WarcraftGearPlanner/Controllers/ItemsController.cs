using Microsoft.AspNetCore.Mvc;
using WarcraftGearPlanner.Models;
using WarcraftGearPlanner.Services;

namespace WarcraftGearPlanner.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ItemsController(IBattleNetService battleNetService) : ControllerBase
{
	private readonly IBattleNetService _battleNetService = battleNetService;

	[HttpGet("{id}")]
	[ProducesResponseType<MediaReference>(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<MediaReference>> GetItemMedia(int id)
	{
		var itemMedia = await _battleNetService.GetItemMedia(id);
		return itemMedia is null ? NotFound() : Ok(itemMedia);
	}

	[HttpGet]
	[ProducesResponseType<IEnumerable<MediaReference>>(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<IEnumerable<MediaReference>>> GetItemMedia([FromQuery] int[] ids)
	{
		var itemMedia = await _battleNetService.GetItemMedia(ids);
		return itemMedia is null ? NotFound() : Ok(itemMedia);
	}
}
