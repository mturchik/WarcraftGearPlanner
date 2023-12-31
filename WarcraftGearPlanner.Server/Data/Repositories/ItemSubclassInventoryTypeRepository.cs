using WarcraftGearPlanner.Server.Data.Entities;

namespace WarcraftGearPlanner.Server.Data.Repositories;

public class ItemSubclassInventoryTypeRepository(ApplicationDbContext context) : Repository<ItemSubclassInventoryTypeEntity>(context)
{
}
