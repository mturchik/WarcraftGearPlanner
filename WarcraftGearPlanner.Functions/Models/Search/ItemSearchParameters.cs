namespace WarcraftGearPlanner.Functions.Models.Search;

public class ItemSearchParameters : BaseSearchParameters
{
	public int? MinId { get; set; }
	public int? MaxId { get; set; }
	public int? ItemClassId { get; set; }
	public int? ItemSubclassId { get; set; }
	public string? InventoryType { get; set; }
	public string? Quality { get; set; }

	public override List<string> ToCriterium()
	{
		var criterium = new List<string>();

		if (MinId.HasValue || MaxId.HasValue)
			criterium.Add($"id=[{MinId},{MaxId}]");

		if (ItemClassId.HasValue)
			criterium.Add($"item_class.id={ItemClassId}");

		if (ItemSubclassId.HasValue)
			criterium.Add($"item_subclass.id={ItemSubclassId}");

		if (!string.IsNullOrEmpty(InventoryType))
			criterium.Add($"inventory_type.type={InventoryType}");

		if (!string.IsNullOrEmpty(Quality))
			criterium.Add($"quality.type={Quality}");

		return criterium;
	}
}
