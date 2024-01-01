using Microsoft.EntityFrameworkCore;
using WarcraftGearPlanner.Server.Data.Entities;

namespace WarcraftGearPlanner.Server.Data.Repositories;

public class InventoryTypeRepository(ApplicationDbContext context) : Repository<InventoryTypeEntity>(context)
{
	public override IQueryable<InventoryTypeEntity> AddIncludes(IQueryable<InventoryTypeEntity> query)
	{
		query = query.Include(x => x.ItemSubclassInventoryTypes);
		return query;
	}
}
