using WarcraftGearPlanner.Shared.Requests.Search;

namespace WarcraftGearPlanner.Functions.Models.Search;

public class ItemSearchParameters : ISearchParameters
{
	public int? MinId { get; set; }
	public int? MaxId { get; set; }
	public int? ItemClassId { get; set; }
	public int? ItemSubclassId { get; set; }
	public string? InventoryType { get; set; }
	public string? Quality { get; set; }

	public List<string> ToQueryParameters()
	{
		var parameters = new List<string>();

		if (MinId.HasValue || MaxId.HasValue)
			parameters.Add($"id=[{MinId},{MaxId}]");

		if (ItemClassId.HasValue)
			parameters.Add($"item_class.id={ItemClassId}");

		if (ItemSubclassId.HasValue)
			parameters.Add($"item_subclass.id={ItemSubclassId}");

		if (!string.IsNullOrEmpty(InventoryType))
			parameters.Add($"inventory_type.type={InventoryType}");

		if (!string.IsNullOrEmpty(Quality))
			parameters.Add($"quality.type={Quality}");

		return parameters;
	}
}
