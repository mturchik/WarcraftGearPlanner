using Microsoft.AspNetCore.Mvc;
using WarcraftGearPlanner.Server.Services.Items;
using WarcraftGearPlanner.Shared.Models.Items;

namespace WarcraftGearPlanner.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ItemsController(IItemService itemService) : ControllerBase
{
	private readonly IItemService itemService = itemService;

	[HttpPut("search-results")]
	[ProducesResponseType<List<Item>>(StatusCodes.Status200OK)]
	public async Task<ActionResult<List<Item>>> PutItemSearchResults([FromBody] List<Item> items)
	{
		var updatedItems = await itemService.MergeSearchResults(items);
		return Ok(updatedItems);
	}
}
