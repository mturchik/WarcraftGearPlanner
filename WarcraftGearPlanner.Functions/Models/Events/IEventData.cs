namespace WarcraftGearPlanner.Functions.Models.Events;
public interface IEventData
{
	[JsonIgnore]
	string LogString { get; }
}
