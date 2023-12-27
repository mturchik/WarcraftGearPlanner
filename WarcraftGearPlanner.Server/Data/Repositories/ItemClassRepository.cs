using Microsoft.EntityFrameworkCore;
using WarcraftGearPlanner.Server.Data.Entities;

namespace WarcraftGearPlanner.Server.Data.Repositories;

public class ItemClassRepository(ApplicationDbContext context) : Repository<ItemClassEntity>(context)
{
	public override async Task<ItemClassEntity?> GetByIdAsync(Guid id)
	{
		var entity = await TableQuery.Include(x => x.Subclasses).FirstOrDefaultAsync(x => x.Id == id);
		return entity;
	}

	public override async Task<List<ItemClassEntity>> GetListAsync()
	{
		var entities = await TableQuery.Include(x => x.Subclasses).ToListAsync();
		return entities;
	}
}
