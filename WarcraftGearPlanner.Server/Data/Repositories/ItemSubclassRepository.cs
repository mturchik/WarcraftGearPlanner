using WarcraftGearPlanner.Server.Data.Entities;

namespace WarcraftGearPlanner.Server.Data.Repositories;

public class ItemSubclassRepository(ApplicationDbContext context) : Repository<ItemSubclassEntity>(context)
{
}
