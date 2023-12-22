using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WarcraftGearPlanner.Server.Data.Entities;

namespace WarcraftGearPlanner.Server.Data.Repositories;

public class ItemClassRepository(ApplicationDbContext context) : Repository<ItemClassEntity>(context)
{
	public override async Task<ItemClassEntity?> GetByIdAsync(Guid id)
	{
		var entity = await Table.Include(x => x.Subclasses).FirstOrDefaultAsync(x => x.Id == id);
		return entity;
	}

	public override async Task<List<ItemClassEntity>> GetListAsync(Expression<Func<ItemClassEntity, bool>>? selector = null)
	{
		var query = TableQuery;
		if (selector != null) query = query.Where(selector);
		var entities = await query.Include(x => x.Subclasses).ToListAsync();
		return entities;
	}
}
