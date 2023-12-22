using Microsoft.AspNetCore.Mvc;
using WarcraftGearPlanner.Server.Services;
using WarcraftGearPlanner.Shared.Models.Items;

namespace WarcraftGearPlanner.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ItemsController(IItemsService itemsService) : ControllerBase
{
	private readonly IItemsService itemsService = itemsService;

	[HttpGet("item-class")]
	[ProducesResponseType<IEnumerable<ItemClass>>(StatusCodes.Status200OK)]
	public async Task<ActionResult<IEnumerable<ItemClass>>> GetItemClasses()
	{
		var itemClasses = await itemsService.GetItemClasses();
		return Ok(itemClasses);
	}

	[HttpGet("item-class/{itemClassId}/item-subclass")]
	[ProducesResponseType<IEnumerable<ItemSubclass>>(StatusCodes.Status200OK)]
	public async Task<ActionResult<IEnumerable<ItemSubclass>>> GetItemSubclasses(Guid itemClassId)
	{
		var itemSubclasses = await itemsService.GetItemSubclasses(itemClassId);
		return Ok(itemSubclasses);
	}
}
