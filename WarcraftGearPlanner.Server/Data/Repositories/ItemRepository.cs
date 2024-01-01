using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WarcraftGearPlanner.Server.Data.Entities;
using WarcraftGearPlanner.Server.Requests.Search;
using WarcraftGearPlanner.Shared.Enum;
using WarcraftGearPlanner.Shared.Requests.Search;

namespace WarcraftGearPlanner.Server.Data.Repositories;

public class ItemRepository(ApplicationDbContext context) : Repository<ItemEntity>(context)
{
	public override IQueryable<ItemEntity> AddIncludes(IQueryable<ItemEntity> query)
	{
		query = query.Include(x => x.ItemClass);
		query = query.Include(x => x.ItemSubclass);
		query = query.Include(x => x.InventoryType);
		query = query.Include(x => x.ItemQuality);
		return query;
	}

	public override IQueryable<ItemEntity> AddOrderBy<TParameters>(IQueryable<ItemEntity> query, SearchRequest<TParameters> searchRequest)
	{
		var direction = searchRequest.OrderDirection ?? OrderDirection.Asc;
		IOrderedQueryable<ItemEntity> orderByDirection<TKey>(Expression<Func<ItemEntity, TKey?>> keySelector)
			=> direction == OrderDirection.Asc ? query.OrderBy(keySelector) : query.OrderByDescending(keySelector);

		var property = searchRequest.OrderProperty?.ToLower();
		query = (property) switch
		{
			"name" => orderByDirection(x => x.Name),
			"level" => orderByDirection(x => x.Level),
			"requiredlevel" => orderByDirection(x => x.RequiredLevel),
			"itemclass" => orderByDirection(x => x.ItemClass!.Name),
			"itemsubclass" => orderByDirection(x => x.ItemSubclass!.Name),
			"itemquality" => orderByDirection(x => x.ItemQuality!.Name),
			"inventorytype" => orderByDirection(x => x.InventoryType!.Name),
			_ => orderByDirection(x => x.Id.ToString()),
		};

		return query;
	}

	public override IQueryable<ItemEntity> AddFilters<TParameters>(IQueryable<ItemEntity> query, SearchRequest<TParameters> searchRequest)
	{
		if (searchRequest.Parameters is not ItemSearchParameters parameters) return query;

		if (!string.IsNullOrEmpty(parameters.Name))
			query = query.Where(x => x.Name!.Contains(parameters.Name));

		if (parameters.ItemLevelMin.HasValue)
			query = query.Where(x => x.Level >= parameters.ItemLevelMin);
		if (parameters.ItemLevelMax.HasValue)
			query = query.Where(x => x.Level <= parameters.ItemLevelMax);

		if (parameters.RequiredLevelMin.HasValue)
			query = query.Where(x => x.RequiredLevel >= parameters.RequiredLevelMin);
		if (parameters.RequiredLevelMax.HasValue)
			query = query.Where(x => x.RequiredLevel <= parameters.RequiredLevelMax);

		if (parameters.ItemClassIds.Count != 0)
			query = query.Where(x => parameters.ItemClassIds.Contains(x.ItemClassId));

		if (parameters.ItemSubclassIds.Count != 0)
			query = query.Where(x => parameters.ItemSubclassIds.Contains(x.ItemSubclassId));

		if (parameters.ItemQualityIds.Count != 0)
			query = query.Where(x => parameters.ItemQualityIds.Contains(x.ItemQualityId));

		if (parameters.InventoryIds.Count != 0)
			query = query.Where(x => parameters.InventoryIds.Contains(x.InventoryTypeId));

		return query;
	}
}
