namespace WarcraftGearPlanner.Shared.Models;
public abstract class BaseModel
{
	public Guid Id { get; set; }
	public DateTime CreatedAt { get; private set; }
}
