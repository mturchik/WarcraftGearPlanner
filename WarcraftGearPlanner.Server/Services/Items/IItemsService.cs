using WarcraftGearPlanner.Server.Data.Entities;

namespace WarcraftGearPlanner.Server.Services.Items;

public interface IItemsService
{
    Task<List<ItemClassEntity>> GetItemClasses();
    Task<List<ItemSubclassEntity>> GetItemSubclasses(Guid itemClassId);
}
