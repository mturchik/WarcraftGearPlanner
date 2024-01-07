using Microsoft.AspNetCore.Mvc;
using WarcraftGearPlanner.Server.Requests.Search;
using WarcraftGearPlanner.Server.Services.Items;
using WarcraftGearPlanner.Shared.Models.Items;
using WarcraftGearPlanner.Shared.Requests.Search;

namespace WarcraftGearPlanner.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ItemsController(IItemService itemService) : ControllerBase
{
	private readonly IItemService itemService = itemService;

	[HttpGet("count")]
	[ProducesResponseType<int>(StatusCodes.Status200OK)]
	public async Task<ActionResult<int>> GetItemCount(
		[FromQuery] List<Guid?> itemClassIds,
		[FromQuery] List<Guid?> itemSubclassIds
	)
	{
		var count = await itemService.GetCountAsync(itemClassIds, itemSubclassIds);
		return Ok(count);
	}

	[HttpPost("search-results")]
	[ProducesResponseType<SearchResponse<Item>>(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<SearchResponse<Item>>> SearchItems([FromBody] SearchRequest<ItemSearchParameters> searchRequest)
	{
		var items = await itemService.SearchItems(searchRequest);
		return items is null ? BadRequest() : Ok(items);
	}

	[HttpPut("search-results")]
	[ProducesResponseType<List<Item>>(StatusCodes.Status200OK)]
	public async Task<ActionResult<List<Item>>> PutItemSearchResults([FromBody] List<Item> items)
	{
		var updatedItems = await itemService.MergeSearchResults(items);
		return Ok(updatedItems);
	}
}
