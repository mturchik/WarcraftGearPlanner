namespace WarcraftGearPlanner.Functions.Models.Search;

public class ItemSearchParameters : BaseSearchParameters
{
	public string? Name { get; set; }
	public int? MinItemLevel { get; set; }
	public int? MaxItemLevel { get; set; }
	public int? MinReqLevel { get; set; }
	public int? MaxReqLevel { get; set; }
	public int? ItemClassId { get; set; }
	public int? ItemSubclassId { get; set; }
	public int? InventoryTypeId { get; set; }

	public override List<string> ToCriterium()
	{
		var criterium = new List<string>();

		if (!string.IsNullOrWhiteSpace(Name))
			criterium.Add($"name.en_US={Name}");

		if (MinItemLevel.HasValue || MaxItemLevel.HasValue)
			criterium.Add($"level=[{MinItemLevel},{MaxItemLevel}]");

		if (MinReqLevel.HasValue || MaxReqLevel.HasValue)
			criterium.Add($"required_level=[{MinReqLevel},{MaxReqLevel}]");

		if (ItemClassId.HasValue)
			criterium.Add($"item_class.id={ItemClassId}");

		if (ItemSubclassId.HasValue)
			criterium.Add($"item_subclass.id={ItemSubclassId}");

		if (InventoryTypeId.HasValue)
			criterium.Add($"inventory_type.id={InventoryTypeId}");

		return criterium;
	}
}
