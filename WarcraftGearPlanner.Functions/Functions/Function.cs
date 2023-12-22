using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using WarcraftGearPlanner.Functions.Services;

namespace WarcraftGearPlanner.Functions.Functions;

public class Function
{
	private readonly IBattleNetService _battleNetService;

	public Function(IBattleNetService battleNetService)
	{
		_battleNetService = battleNetService;
	}

	[FunctionName("Function")]
	public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
	{
		var realms = await _battleNetService.GetRealmIndex();

		var itemClasses = await _battleNetService.GetItemClassIndex();

		var itemClassId = itemClasses?.ItemClasses.FirstOrDefault()?.Id ?? 0;
		var itemClass = await _battleNetService.GetItemClass(itemClassId);

		var responseMessage = $"Realms: {realms?.Realms.Count} | ItemClasses: {itemClasses?.ItemClasses.Count} | ItemClass: {itemClass is not null}";

		return new OkObjectResult(responseMessage);
	}
}
