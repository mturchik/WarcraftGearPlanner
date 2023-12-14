using Microsoft.AspNetCore.Mvc;
using WarcraftGearPlanner.Server.Models;
using WarcraftGearPlanner.Server.Services;

namespace WarcraftGearPlanner.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CharactersController(IBattleNetService battleNetService) : ControllerBase
{
	private readonly IBattleNetService _battleNetService = battleNetService;

	[HttpGet("{realmSlug}/{characterName}")]
	[ProducesResponseType<CharacterProfile>(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<CharacterProfile>> GetCharacter(string realmSlug, string characterName)
	{
		var character = await _battleNetService.GetCharacterProfile(realmSlug, characterName);
		return character is null ? NotFound() : Ok(character);
	}

	[HttpGet("{realmSlug}/{characterName}/equipment")]
	[ProducesResponseType<EquipmentSummary>(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<EquipmentSummary>> GetEquipment(string realmSlug, string characterName)
	{
		var equipment = await _battleNetService.GetEquipmentSummary(realmSlug, characterName);
		return equipment is null ? NotFound() : Ok(equipment);
	}
}
