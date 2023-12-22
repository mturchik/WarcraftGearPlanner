using WarcraftGearPlanner.Server.Data.Entities;

namespace WarcraftGearPlanner.Server.Services;

public interface IItemsService
{
	Task<List<ItemClassEntity>> GetItemClasses();
	Task<List<ItemSubclassEntity>> GetItemSubclasses(Guid itemClassId);
}
