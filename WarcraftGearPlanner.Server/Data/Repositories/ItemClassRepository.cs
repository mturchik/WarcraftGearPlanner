﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WarcraftGearPlanner.Server.Data.Entities;

namespace WarcraftGearPlanner.Server.Data.Repositories;

public class ItemClassRepository(ApplicationDbContext context) : Repository<ItemClassEntity>(context)
{
	public override async Task<ItemClassEntity?> GetByIdAsync(Guid id)
	{
		var entity = await TableQuery
			.Include(x => x.Subclasses)!.ThenInclude(x => x.ItemSubclassInventoryTypes)!.ThenInclude(x => x.InventoryType)
			.FirstOrDefaultAsync(x => x.Id == id);
		return entity;
	}

	public override async Task<List<ItemClassEntity>> GetListAsync(Expression<Func<ItemClassEntity, bool>>? selector)
	{
		var query = TableQuery;
		if (selector is not null) query = query.Where(selector);

		var entities = await query
			.Include(x => x.Subclasses)!.ThenInclude(x => x.ItemSubclassInventoryTypes)!.ThenInclude(x => x.InventoryType)
			.ToListAsync();
		return entities;
	}
}
