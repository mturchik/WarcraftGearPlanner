namespace WarcraftGearPlanner.Shared.Models.Items;
public class InventoryType : BaseModel
{
	public string Type { get; set; } = "";
	public string Name { get; set; } = "";
	public int? DisplayOrder { get; set; }
}
