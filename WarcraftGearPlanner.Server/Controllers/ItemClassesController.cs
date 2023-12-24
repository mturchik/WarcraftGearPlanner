using Microsoft.AspNetCore.Mvc;
using WarcraftGearPlanner.Server.Services.Items;
using WarcraftGearPlanner.Shared.Models.Items;

namespace WarcraftGearPlanner.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ItemClassesController(IItemClassService itemClassService) : ControllerBase
{
	private readonly IItemClassService itemClassService = itemClassService;

	[HttpGet]
	[ProducesResponseType<List<ItemClass>>(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<List<ItemClass>>> GetItemClasses()
	{
		var itemClasses = await itemClassService.GetListAsync();
		return Ok(itemClasses);
	}

	[HttpPost]
	[ProducesResponseType<List<ItemClass>>(StatusCodes.Status200OK)]
	public async Task<ActionResult<List<ItemClass>>> PostItemClasses([FromBody] List<ItemClass> itemClasses)
	{
		var createdItemClasses = await itemClassService.CreateListAsync(itemClasses);
		return Ok(createdItemClasses);
	}

	[HttpPut]
	[ProducesResponseType<List<ItemClass>>(StatusCodes.Status200OK)]
	public async Task<ActionResult<List<ItemClass>>> PutItemClasses([FromBody] List<ItemClass> itemClasses)
	{
		var updatedItemClasses = await itemClassService.UpdateListAsync(itemClasses);
		return Ok(updatedItemClasses);
	}

	[HttpDelete]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult> DeleteItemClasses([FromQuery] List<Guid> ids)
	{
		await itemClassService.DeleteListAsync(ids);
		return Ok();
	}
}
