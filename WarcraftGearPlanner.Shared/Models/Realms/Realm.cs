namespace WarcraftGearPlanner.Shared.Models.Realms;
public class Realm : BaseModel
{
	public int RealmId { get; set; }
	public string? Name { get; set; }
	public string? Slug { get; set; }
}
