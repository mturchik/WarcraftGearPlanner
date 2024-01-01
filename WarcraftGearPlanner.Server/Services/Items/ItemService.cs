using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using WarcraftGearPlanner.Server.Data;
using WarcraftGearPlanner.Server.Data.Entities;
using WarcraftGearPlanner.Server.Requests.Search;
using WarcraftGearPlanner.Shared.Models.Items;
using WarcraftGearPlanner.Shared.Requests.Search;

namespace WarcraftGearPlanner.Server.Services.Items;

public class ItemService(
	IRepository<ItemEntity> repository,
	IValidator<Item> validator,
	IMemoryCache memoryCache,
	IMapper mapper,
	IRepository<ItemQualityEntity> itemQualityRepository,
	IRepository<InventoryTypeEntity> inventoryTypeRepository,
	IRepository<ItemSubclassInventoryTypeEntity> itemSubclassInventoryTypeRepository
) : BaseService<Item, ItemEntity>(repository, validator, memoryCache, mapper), IItemService
{
	private readonly IRepository<ItemQualityEntity> itemQualityRepository = itemQualityRepository;
	private readonly IRepository<InventoryTypeEntity> inventoryTypeRepository = inventoryTypeRepository;
	private readonly IRepository<ItemSubclassInventoryTypeEntity> itemSubclassInventoryTypeRepository = itemSubclassInventoryTypeRepository;

	#region Search Items

	public async Task<SearchResponse<Item>?> SearchItems(SearchRequest<ItemSearchParameters> searchRequest)
	{
		var (entities, count) = await repository.SearchAsync(searchRequest);
		var items = mapper.Map<List<Item>>(entities);

		return SearchResponse<Item>.FromRequestResults(searchRequest, items, count);
	}

	#endregion Search Items

	#region Merge Search Results

	public async Task<List<Item>> MergeSearchResults(List<Item> models)
	{
		foreach (var model in models) await validator.ValidateAndThrowAsync(model);

		var itemIds = models.Select(m => m.ItemId).Distinct();

		var entities = await repository.GetListAsync(e => itemIds.Contains(e.ItemId));

		var itemQualities = await GetOrCreateItemQualities(models);
		var inventoryTypes = await GetOrCreateInventoryTypes(models);

		List<ItemEntity> toCreate = [];
		foreach (var model in models)
		{
			if (model.ItemQualityId == Guid.Empty && !string.IsNullOrEmpty(model.ItemQualityType))
			{
				var itemQuality = itemQualities.First(t => t.Type == model.ItemQualityType);
				model.ItemQualityId = itemQuality.Id;
			}

			if (model.InventoryTypeId == Guid.Empty && !string.IsNullOrEmpty(model.InventoryTypeType))
			{
				var inventoryType = inventoryTypes.First(t => t.Type == model.InventoryTypeType);
				model.InventoryTypeId = inventoryType.Id;
			}

			await CheckItemSubclassInventoryTypeRelationship(inventoryTypes, model);

			var entity = entities.FirstOrDefault(e => e.ItemId == model.ItemId);
			if (entity is null)
			{
				entity = mapper.Map<ItemEntity>(model);
				toCreate.Add(entity);
			}
			else
			{
				model.Id = entity.Id;
				mapper.Map(model, entity);
			}
		}

		if (entities.Count > 0)
			entities = await repository.UpdateListAsync(entities);

		if (toCreate.Count > 0)
		{
			await repository.CreateListAsync(toCreate);
			entities.AddRange(toCreate);
		}

		memoryCache.Remove(cacheKey);

		var updatedModels = mapper.Map<List<Item>>(entities);
		return updatedModels;
	}

	private async Task<List<ItemQualityEntity>> GetOrCreateItemQualities(List<Item> items)
	{
		var itemQualities = items
			.Select(i => new ItemQualityEntity
			{
				Type = i.ItemQualityType ?? "",
				Name = i.ItemQualityName ?? ""
			})
			.DistinctBy(q => $"{q.Type}-{q.Name}")
			.ToList();

		var types = itemQualities.Select(m => m.Type).ToList();
		var entities = await itemQualityRepository.GetListAsync(e => types.Contains(e.Type));

		var toCreate = itemQualities.Where(i => !entities.Any(e => e.Type == i.Type)).ToList();

		var created = await itemQualityRepository.CreateListAsync(toCreate);
		memoryCache.Remove(typeof(ItemQualityEntity).Name);

		entities.AddRange(created);

		return entities;
	}

	private async Task<List<InventoryTypeEntity>> GetOrCreateInventoryTypes(List<Item> items)
	{
		var inventoryTypes = items
			.Select(i => new InventoryTypeEntity
			{
				Type = i.InventoryTypeType ?? "",
				Name = i.InventoryTypeName ?? ""
			})
			.DistinctBy(q => $"{q.Type}-{q.Name}")
			.ToList();

		var types = inventoryTypes.Select(m => m.Type).ToList();
		var entities = await inventoryTypeRepository.GetListAsync(e => types.Contains(e.Type));

		var toCreate = inventoryTypes.Where(i => !entities.Any(e => e.Type == i.Type)).ToList();

		var created = await inventoryTypeRepository.CreateListAsync(toCreate);
		memoryCache.Remove(typeof(InventoryTypeEntity).Name);

		entities.AddRange(created);

		return entities;
	}

	private async Task CheckItemSubclassInventoryTypeRelationship(List<InventoryTypeEntity> inventoryTypes, Item item)
	{
		var inventoryType = inventoryTypes.First(t => t.Id == item.InventoryTypeId);
		inventoryType.ItemSubclassInventoryTypes ??= [];

		var existing = inventoryType.ItemSubclassInventoryTypes.FirstOrDefault(i => i.ItemSubclassId == item.ItemSubclassId);
		if (existing is not null) return;

		var itemSubclassInventoryType = new ItemSubclassInventoryTypeEntity
		{
			InventoryTypeId = inventoryType.Id,
			ItemSubclassId = item.ItemSubclassId
		};
		await itemSubclassInventoryTypeRepository.CreateAsync(itemSubclassInventoryType);
		memoryCache.Remove(typeof(ItemClassEntity).Name);
	}

	#endregion Merge Search Results
}
