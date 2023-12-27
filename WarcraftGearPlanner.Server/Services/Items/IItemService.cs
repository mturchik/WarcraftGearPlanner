using WarcraftGearPlanner.Server.Data.Entities;
using WarcraftGearPlanner.Shared.Models.Items;

namespace WarcraftGearPlanner.Server.Services.Items;

public interface IItemService : IService<Item, ItemEntity>
{
	Task<List<Item>> MergeSearchResults(List<Item> models);
}
