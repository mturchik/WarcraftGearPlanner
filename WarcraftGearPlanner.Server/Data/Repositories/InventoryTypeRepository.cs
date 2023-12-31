using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WarcraftGearPlanner.Server.Data.Entities;

namespace WarcraftGearPlanner.Server.Data.Repositories;

public class InventoryTypeRepository(ApplicationDbContext context) : Repository<InventoryTypeEntity>(context)
{
	public override async Task<List<InventoryTypeEntity>> GetListAsync(Expression<Func<InventoryTypeEntity, bool>>? selector)
	{
		var query = TableQuery;
		if (selector is not null) query = query.Where(selector);
		var entities = await query
			.Include(x => x.ItemSubclassInventoryTypes)
			.ToListAsync();
		return entities;
	}
}
