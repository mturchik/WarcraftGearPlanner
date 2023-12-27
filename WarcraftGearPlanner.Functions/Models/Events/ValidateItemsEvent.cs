namespace WarcraftGearPlanner.Functions.Models.Events;
public class ValidateItemsEvent
{
	public Guid ItemClassId { get; set; }
	public Guid ItemSubclassId { get; set; }

	public int ClassId { get; set; }
	public int SubclassId { get; set; }
}
