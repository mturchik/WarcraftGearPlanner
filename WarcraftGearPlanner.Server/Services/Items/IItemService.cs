using WarcraftGearPlanner.Server.Data.Entities;
using WarcraftGearPlanner.Server.Requests.Search;
using WarcraftGearPlanner.Shared.Models.Items;
using WarcraftGearPlanner.Shared.Requests.Search;

namespace WarcraftGearPlanner.Server.Services.Items;

public interface IItemService : IService<Item, ItemEntity>
{
	Task<int> GetCountAsync(List<Guid?> itemClassIds, List<Guid?> itemSubclassIds);
	Task<List<Item>> MergeSearchResults(List<Item> models);
	Task<SearchResponse<Item>?> SearchItems(SearchRequest<ItemSearchParameters> searchRequest);
}
