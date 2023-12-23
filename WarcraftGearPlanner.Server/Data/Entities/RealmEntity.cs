namespace WarcraftGearPlanner.Server.Data.Entities;

public class RealmEntity : BaseEntity
{
	public int RealmId { get; set; }
	public string? Name { get; set; }
	public string? Slug { get; set; }
}
