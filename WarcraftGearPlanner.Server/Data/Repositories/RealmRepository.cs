using WarcraftGearPlanner.Server.Data.Entities;

namespace WarcraftGearPlanner.Server.Data.Repositories;

public class RealmRepository(ApplicationDbContext context) : Repository<RealmEntity>(context)
{
}
