using Microsoft.AspNetCore.Mvc;
using WarcraftGearPlanner.Server.Services.ItemQualities;
using WarcraftGearPlanner.Shared.Models.Items;

namespace WarcraftGearPlanner.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ItemQualitiesController(IItemQualityService itemQualityService) : ControllerBase
{
	private readonly IItemQualityService itemQualityService = itemQualityService;

	[HttpGet]
	[ProducesResponseType<List<ItemQuality>>(StatusCodes.Status200OK)]
	public async Task<ActionResult<List<ItemQuality>>> GetItemQualities()
	{
		var itemQualities = await itemQualityService.GetListAsync();
		return Ok(itemQualities);
	}
}
