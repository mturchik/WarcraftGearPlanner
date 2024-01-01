using Microsoft.EntityFrameworkCore;
using WarcraftGearPlanner.Server.Data.Entities;

namespace WarcraftGearPlanner.Server.Data.Repositories;

public class ItemClassRepository(ApplicationDbContext context) : Repository<ItemClassEntity>(context)
{
	public override IQueryable<ItemClassEntity> AddIncludes(IQueryable<ItemClassEntity> query)
	{
		query = query.Include(x => x.Subclasses)!.ThenInclude(x => x.ItemSubclassInventoryTypes)!.ThenInclude(x => x.InventoryType);
		return query;
	}
}
