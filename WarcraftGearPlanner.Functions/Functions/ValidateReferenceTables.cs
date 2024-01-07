using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using WarcraftGearPlanner.Functions.Services;
using ItemClassDTO = WarcraftGearPlanner.Shared.Models.Items.ItemClass;
using ItemSubclassDTO = WarcraftGearPlanner.Shared.Models.Items.ItemSubclass;
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
	public async Task Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req, ILogger logger)
	{
		if (req is null) throw new ArgumentNullException(nameof(req));

		_logger = logger;
		_logger.LogInformation($"ValidateReferenceTables executed at: {DateTime.Now}");

		_logger.LogInformation("Validating reference tables...");

		await ValidateRealms();
		await ValidateItemClasses();

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
		var toUpdate = new List<RealmDTO>();
		var toDelete = existing.Where(e => realmDTOs.All(r => r.RealmId != e.RealmId)).ToList();

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

		if (toCreate.Count > 0)
		{
			_logger.LogInformation($"Creating {toCreate.Count} realms...");
			var created = await _apiService.Post("/realms", toCreate) ?? new();
			_logger.LogInformation($"Created {created.Count} realms");
		}

		if (toUpdate.Count > 0)
		{
			_logger.LogInformation($"Updating {toUpdate.Count} realms...");
			var updated = await _apiService.Put("/realms", toUpdate) ?? new();
			_logger.LogInformation($"Updated {updated.Count} realms");
		}

		if (toDelete.Count > 0)
		{
			_logger.LogInformation($"Deleting {toDelete.Count} realms...");
			var query = string.Join("&", toDelete.Select(r => $"ids={r.Id}"));
			await _apiService.Delete($"/realms?{query}");
			_logger.LogInformation($"Deleted realms");
		}

		_logger.LogInformation("Finished validating realms");
	}

	private async Task ValidateItemClasses()
	{
		_logger.LogInformation("Validating item classes...");

		var itemClassIndex = await _battleNetService.GetItemClassIndex();
		var itemClassDTOs = itemClassIndex?.ItemClasses
			.Where(c => c.Id is not null)
			.Select(c => new ItemClassDTO
			{
				ClassId = c.Id ?? 0,
				Name = c.Name
			}).ToList() ?? new();
		_logger.LogInformation($"Retrieved {itemClassDTOs.Count} item classes from Battle.net");

		var existing = await _apiService.Get<List<ItemClassDTO>>("/item-classes") ?? new();
		_logger.LogInformation($"Retrieved {existing.Count} item classes from API");

		var toCreate = itemClassDTOs.Where(r => existing.All(e => e.ClassId != r.ClassId)).ToList();
		var toUpdate = new List<ItemClassDTO>();
		var toDelete = existing.Where(e => itemClassDTOs.All(r => r.ClassId != e.ClassId)).ToList();

		foreach (var itemClassDTO in itemClassDTOs)
		{
			var itemSubclassIndexes = (await _battleNetService.GetItemClass(itemClassDTO.ClassId))?.Subclasses ?? new();
			var itemSubclassTasks = itemSubclassIndexes
				.Where(s => s.Id is not null)
				.Select(s => _battleNetService.GetItemSubclass(itemClassDTO.ClassId, s.Id ?? 0));
			var itemSubclassDTOs = (await Task.WhenAll(itemSubclassTasks))
				.Where(s => s is not null)
				.Select(s => new ItemSubclassDTO
				{
					SubclassId = s?.Id ?? 0,
					Name = s?.Name,
					VerboseName = s?.VerboseName,
					HideTooltip = s?.HideTooltip ?? false
				}).ToList();

			itemClassDTO.Subclasses ??= new();
			var existingItemClass = existing.FirstOrDefault(e => e.ClassId == itemClassDTO.ClassId);
			if (existingItemClass is null)
				itemClassDTO.Subclasses.AddRange(itemSubclassDTOs);
			else
			{
				itemClassDTO.Id = existingItemClass.Id;
				itemClassDTO.DisplayOrder = existingItemClass.DisplayOrder;

				var subclassChanges = false;
				foreach (var itemSubclassDTO in itemSubclassDTOs)
				{
					var existingItemSubclass = existingItemClass.Subclasses?.FirstOrDefault(s => s.SubclassId == itemSubclassDTO.SubclassId);
					if (existingItemSubclass is null)
					{
						itemClassDTO.Subclasses.Add(itemSubclassDTO);
						subclassChanges = true;
					}
					else
					{
						itemSubclassDTO.Id = existingItemSubclass.Id;
						itemSubclassDTO.DisplayOrder = existingItemSubclass.DisplayOrder;

						var itemSubclassItemsCount = await _apiService.Get<int>($"/items/count?itemSubclassIds={itemSubclassDTO.Id}");
						_logger.LogInformation($"Counted {itemSubclassItemsCount} items for item subclass {itemSubclassDTO.Name}");
						if (itemSubclassItemsCount == 0) itemSubclassDTO.DisplayOrder = -1;

						if (existingItemSubclass.Name != itemSubclassDTO.Name
							|| existingItemSubclass.VerboseName != itemSubclassDTO.VerboseName
							|| existingItemSubclass.HideTooltip != itemSubclassDTO.HideTooltip
							|| existingItemSubclass.DisplayOrder != itemSubclassDTO.DisplayOrder)
							subclassChanges = true;

						itemClassDTO.Subclasses.Add(itemSubclassDTO);
					}
				}

				var itemClassItemsCount = await _apiService.Get<int>($"/items/count?itemClassIds={itemClassDTO.Id}");
				_logger.LogInformation($"Counted {itemClassItemsCount} items for item class {itemClassDTO.Name}");
				if (itemClassItemsCount == 0) itemClassDTO.DisplayOrder = -1;

				if (existingItemClass.Name != itemClassDTO.Name
					|| existingItemClass.DisplayOrder != itemClassDTO.DisplayOrder
					|| subclassChanges)
				{
					toUpdate.Add(itemClassDTO);
				}
			}
		}

		if (toCreate.Count > 0)
		{
			_logger.LogInformation($"Creating {toCreate.Count} item classes...");
			var created = await _apiService.Post("/item-classes", toCreate) ?? new();
			_logger.LogInformation($"Created {created.Count} item classes");
		}

		if (toUpdate.Count > 0)
		{
			_logger.LogInformation($"Updating {toUpdate.Count} item classes...");
			var updated = await _apiService.Put("/item-classes", toUpdate) ?? new();
			_logger.LogInformation($"Updated {updated.Count} item classes");
		}

		if (toDelete.Count > 0)
		{
			_logger.LogInformation($"Deleting {toDelete.Count} item classes...");
			var query = string.Join("&", toDelete.Select(r => $"ids={r.Id}"));
			await _apiService.Delete($"/item-classes?{query}");
			_logger.LogInformation($"Deleted item classes");
		}

		_logger.LogInformation("Finished validating item classes");
	}
}
