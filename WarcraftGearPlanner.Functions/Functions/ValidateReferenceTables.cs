using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using WarcraftGearPlanner.Functions.Services;
using RealmDTO = WarcraftGearPlanner.Shared.Models.Realms.Realm;

namespace WarcraftGearPlanner.Functions.Functions;

public class ValidateReferenceTables
{
	private readonly IBattleNetService _battleNetService;
	private readonly IApiService _apiService;
	private ILogger _logger = null!;

	public ValidateReferenceTables(IBattleNetService battleNetService, IApiService apiService)
	{
		_battleNetService = battleNetService;
		_apiService = apiService;
	}

	[FunctionName("ValidateReferenceTables")]
	public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger logger)
	{
		_logger = logger;
		_logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

		_logger.LogInformation("Validating reference tables...");

		await ValidateRealms();

		_logger.LogInformation("Finished validating reference tables");
	}

	private async Task ValidateRealms()
	{
		_logger.LogInformation("Validating realms...");

		var realmIndex = await _battleNetService.GetRealmIndex();
		var realmDTOs = realmIndex?.Realms
			.Where(r => r.Id is not null)
			.Select(r => new RealmDTO
			{
				RealmId = r.Id ?? 0,
				Slug = r.Slug,
				Name = r.Name
			}).ToList() ?? new();
		_logger.LogInformation($"Retrieved {realmDTOs.Count} realms from Battle.net");

		var existing = await _apiService.Get<List<RealmDTO>>("/realms") ?? new();
		_logger.LogInformation($"Retrieved {existing.Count} realms from API");

		var toCreate = realmDTOs.Where(r => existing.All(e => e.RealmId != r.RealmId)).ToList();
		if (toCreate.Count > 0)
		{
			_logger.LogInformation($"Creating {toCreate.Count} realms...");
			var created = await _apiService.Post("/realms", toCreate) ?? new();
			_logger.LogInformation($"Created {created.Count} realms");
		}

		var toUpdate = new List<RealmDTO>();
		foreach (var realmDTO in realmDTOs)
		{
			var existingRealm = existing.FirstOrDefault(e => e.RealmId == realmDTO.RealmId);
			if (existingRealm is null) continue;

			if (existingRealm.Name != realmDTO.Name
				|| existingRealm.Slug != realmDTO.Slug)
			{
				realmDTO.Id = existingRealm.Id;
				toUpdate.Add(realmDTO);
			}
		}
		if (toUpdate.Count > 0)
		{
			_logger.LogInformation($"Updating {toUpdate.Count} realms...");
			var updated = await _apiService.Put("/realms", toUpdate) ?? new();
			_logger.LogInformation($"Updated {updated.Count} realms");
		}

		var toDelete = existing.Where(e => realmDTOs.All(r => r.RealmId != e.RealmId)).ToList();
		if (toDelete.Count > 0)
		{
			_logger.LogInformation($"Deleting {toDelete.Count} realms...");
			var query = string.Join("&", toDelete.Select(r => $"ids={r.Id}"));
			await _apiService.Delete($"/realms?{query}");
			_logger.LogInformation($"Deleted realms");
		}

		_logger.LogInformation("Finished validating realms");
	}

}
