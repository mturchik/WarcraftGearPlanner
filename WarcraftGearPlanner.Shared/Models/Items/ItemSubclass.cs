namespace WarcraftGearPlanner.Shared.Models.Items;
public class ItemSubclass : BaseModel
{
	public int SubclassId { get; set; }
	public string? Name { get; set; }
	public string? VerboseName { get; set; }
	public bool HideTooltip { get; set; }
	public List<InventoryType>? InventoryTypes { get; set; }
}
