using WarcraftGearPlanner.Server.Data.Entities;

namespace WarcraftGearPlanner.Server.Data.Repositories;

public class InventoryTypeRepository(ApplicationDbContext context) : Repository<InventoryTypeEntity>(context)
{
}
