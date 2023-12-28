using Microsoft.AspNetCore.Mvc;
using WarcraftGearPlanner.Server.Services.InventoryTypes;
using WarcraftGearPlanner.Shared.Models.Items;

namespace WarcraftGearPlanner.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class InventoryTypesController(IInventoryTypeService inventoryTypeService) : ControllerBase
{
	private readonly IInventoryTypeService inventoryTypeService = inventoryTypeService;

	[HttpGet]
	[ProducesResponseType<List<InventoryType>>(StatusCodes.Status200OK)]
	public async Task<ActionResult<List<InventoryType>>> GetInventoryTypes()
	{
		var inventoryTypes = await inventoryTypeService.GetListAsync();
		return Ok(inventoryTypes);
	}
}
