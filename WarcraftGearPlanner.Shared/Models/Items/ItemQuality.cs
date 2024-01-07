namespace WarcraftGearPlanner.Shared.Models.Items;
public class ItemQuality : BaseModel
{
	public string Type { get; set; } = "";
	public string Name { get; set; } = "";
	public int? DisplayOrder { get; set; }
	public string? Color { get; set; }
}
