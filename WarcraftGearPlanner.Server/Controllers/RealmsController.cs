using Microsoft.AspNetCore.Mvc;
using WarcraftGearPlanner.Server.Services.Realms;
using WarcraftGearPlanner.Shared.Models.Realms;

namespace WarcraftGearPlanner.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RealmsController(IRealmService realmService) : ControllerBase
{
	private readonly IRealmService realmService = realmService;

	[HttpGet]
	[ProducesResponseType<List<Realm>>(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<List<Realm>>> GetRealms()
	{
		var realms = await realmService.GetListAsync();
		return Ok(realms);
	}

	[HttpPost]
	[ProducesResponseType<List<Realm>>(StatusCodes.Status200OK)]
	public async Task<ActionResult<List<Realm>>> PostRealms([FromBody] List<Realm> realms)
	{
		var createdRealms = await realmService.CreateListAsync(realms);
		return Ok(createdRealms);
	}

	[HttpPut]
	[ProducesResponseType<List<Realm>>(StatusCodes.Status200OK)]
	public async Task<ActionResult<List<Realm>>> PutRealms([FromBody] List<Realm> realms)
	{
		var updatedRealms = await realmService.UpdateListAsync(realms);
		return Ok(updatedRealms);
	}

	[HttpDelete]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult> DeleteRealms([FromBody] List<Guid> ids)
	{
		await realmService.DeleteListAsync(ids);
		return Ok();
	}
}
