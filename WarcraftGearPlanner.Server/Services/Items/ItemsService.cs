using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using WarcraftGearPlanner.Server.Data;
using WarcraftGearPlanner.Server.Data.Entities;

namespace WarcraftGearPlanner.Server.Services.Items;

public class ItemsService(
	IRepository<ItemClassEntity> itemClassRepository,
	IRepository<ItemSubclassEntity> itemSubclassRepository,
	IMemoryCache memoryCache,
	IMapper mapper
) : IItemsService
{
	private readonly IRepository<ItemClassEntity> itemClassRepository = itemClassRepository;
	private readonly IRepository<ItemSubclassEntity> itemSubclassRepository = itemSubclassRepository;
	private readonly IMemoryCache memoryCache = memoryCache;
	private readonly IMapper mapper = mapper;

	public async Task<List<ItemClassEntity>> GetItemClasses()
	{
		var itemClassEntities = await memoryCache.GetOrCreateAsync("ItemClassEntities", entry => itemClassRepository.GetListAsync());
		var itemClasses = mapper.Map<List<ItemClassEntity>>(itemClassEntities ?? []);
		return itemClasses;
	}

	public async Task<List<ItemSubclassEntity>> GetItemSubclasses(Guid itemClassId)
	{
		var itemSubclassEntities = await memoryCache.GetOrCreateAsync($"ItemSubclassEntities-{itemClassId}",
			entry => itemSubclassRepository.GetListAsync(itemSubclass => itemSubclass.ItemClassId == itemClassId)
		);
		var itemSubclasses = mapper.Map<List<ItemSubclassEntity>>(itemSubclassEntities ?? []);
		return itemSubclasses;
	}
}
