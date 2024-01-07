namespace WarcraftGearPlanner.Shared.Models.Items;
public class ItemClass : BaseModel
{
	public int ClassId { get; set; }
	public string? Name { get; set; }
	public int? DisplayOrder { get; set; }
	public List<ItemSubclass>? Subclasses { get; set; }
}
