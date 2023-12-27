using WarcraftGearPlanner.Server.Data.Entities;

namespace WarcraftGearPlanner.Server.Data.Repositories;

public class ItemRepository(ApplicationDbContext context) : Repository<ItemEntity>(context)
{
}
