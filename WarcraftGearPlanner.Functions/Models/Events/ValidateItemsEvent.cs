namespace WarcraftGearPlanner.Functions.Models.Events;

public class ValidateItemsEvent : IEventData
{
	public Guid ItemClassId { get; set; }
	public Guid ItemSubclassId { get; set; }
	public Guid ItemQualityId { get; set; }
	public Guid InventoryTypeId { get; set; }

	public int ClassId { get; set; }
	public int SubclassId { get; set; }
	public string? ItemQualityType { get; set; }
	public string? InventoryTypeType { get; set; }

	public int? MinId { get; set; }
	public int? MaxId { get; set; }

	public string LogString =>
		$"Class-{ClassId} | " +
		$"Subclass-{SubclassId} | " +
		$"InventoryType-{InventoryTypeType} | " +
		$"Quality-{ItemQualityType} | " +
		$"Id-[{MinId}-{MaxId}]";
}
