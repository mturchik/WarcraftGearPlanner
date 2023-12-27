using WarcraftGearPlanner.Server.Data.Entities;

namespace WarcraftGearPlanner.Server.Data.Repositories;

public class ItemQualityRepository(ApplicationDbContext context) : Repository<ItemQualityEntity>(context)
{
}
