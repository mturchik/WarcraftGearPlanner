namespace WarcraftGearPlanner.Shared.Models.Items;
public class ItemSubclass
{
	public int SubclassId { get; set; }
	public string? Name { get; set; }
	public Guid ItemClassId { get; set; }
	public ItemClass? ItemClass { get; set; }
}
