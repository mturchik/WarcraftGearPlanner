using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WarcraftGearPlanner.Server.Data.Entities;

public abstract class BaseEntity
{
	[Key, DefaultValue("NEWID()")]
	public Guid Id { get; set; }
	public DateTime CreatedAt { get; private set; }
}
