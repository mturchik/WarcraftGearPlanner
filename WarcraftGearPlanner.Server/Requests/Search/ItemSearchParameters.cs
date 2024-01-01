using WarcraftGearPlanner.Shared.Requests.Search;

namespace WarcraftGearPlanner.Server.Requests.Search;

public class ItemSearchParameters : ISearchParameters
{
	public string? Name { get; set; }
	public int? ItemLevelMin { get; set; }
	public int? ItemLevelMax { get; set; }
	public int? RequiredLevelMin { get; set; }
	public int? RequiredLevelMax { get; set; }
	public List<Guid?> ItemClassIds { get; set; } = [];
	public List<Guid?> ItemSubclassIds { get; set; } = [];
	public List<Guid?> ItemQualityIds { get; set; } = [];
	public List<Guid?> InventoryIds { get; set; } = [];

	public List<string> ToQueryParameters()
	{
		throw new NotImplementedException();
	}
}
