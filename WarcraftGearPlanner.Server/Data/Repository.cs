using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WarcraftGearPlanner.Server.Data.Entities;
using WarcraftGearPlanner.Shared.Enum;
using WarcraftGearPlanner.Shared.Requests.Search;

namespace WarcraftGearPlanner.Server.Data;

public abstract class Repository<TEntity>(ApplicationDbContext context) : IRepository<TEntity> where TEntity : BaseEntity
{
	protected readonly ApplicationDbContext _context = context;

	private DbSet<TEntity> Table => _context.Set<TEntity>();
	protected IQueryable<TEntity> TableQuery => Table.AsQueryable();

	public async Task<TEntity> CreateAsync(TEntity entity)
	{
		await Table.AddAsync(entity);

		await _context.SaveChangesAsync();
		return entity;
	}

	public async Task<List<TEntity>> CreateListAsync(List<TEntity> entities)
	{
		await Table.AddRangeAsync(entities);

		await _context.SaveChangesAsync();
		return entities;
	}

	public async Task DeleteAsync(TEntity entity)
	{
		Table.Remove(entity);
		await _context.SaveChangesAsync();
	}

	public async Task DeleteListAsync(List<TEntity> entities)
	{
		Table.RemoveRange(entities);
		await _context.SaveChangesAsync();
	}

	public virtual async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> selector)
	{
		var entity = await TableQuery.FirstOrDefaultAsync(selector);
		return entity;
	}

	public virtual async Task<TEntity?> GetByIdAsync(Guid id)
	{
		var query = TableQuery;

		query = AddIncludes(query);

		var entity = await query.FirstOrDefaultAsync(x => x.Id == id);
		return entity;
	}

	public virtual Task<int> GetCountAsync() => TableQuery.CountAsync();

	public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? selector)
	{
		var query = TableQuery;

		if (selector is not null) query = query.Where(selector);

		query = AddIncludes(query);

		var entities = await query.ToListAsync();
		return entities;
	}

	public virtual async Task<(List<TEntity> entities, int count)> SearchAsync<TParameters>(SearchRequest<TParameters> searchRequest)
		where TParameters : class, ISearchParameters
	{
		var query = TableQuery;
		query = AddFilters(query, searchRequest);

		var count = await query.CountAsync();

		query = AddOrderBy(query, searchRequest);
		query = AddPaging(query, searchRequest);
		query = AddIncludes(query);

		var entities = await query.ToListAsync();

		return (entities, count);
	}

	public async Task<TEntity> UpdateAsync(TEntity entity)
	{
		// Check to see if the entity has been detached from the context
		if (_context.Entry(entity).State == EntityState.Detached)
			Table.Update(entity);

		await _context.SaveChangesAsync();
		return entity;
	}

	public async Task<List<TEntity>> UpdateListAsync(List<TEntity> entities)
	{
		// Check to see if the entity has been detached from the context
		foreach (var entity in entities)
			if (_context.Entry(entity).State == EntityState.Detached)
				Table.Update(entity);

		await _context.SaveChangesAsync();
		return entities;
	}

	#region Query Helpers

	/// <summary>
	/// This method is used to add includes to the search query. This method will not add any includes by default. Override to add custom includes.
	/// </summary>
	public virtual IQueryable<TEntity> AddIncludes(IQueryable<TEntity> query)
	{
		return query;
	}

	/// <summary>
	/// This method is used to add ordering to the search query. This method is set to order by Id by default. Override to add custom ordering.
	/// </summary>
	/// <typeparam name="TParameters"></typeparam>
	/// <param name="query"></param>
	/// <param name="searchRequest"></param>
	/// <returns></returns>
	public virtual IQueryable<TEntity> AddOrderBy<TParameters>(IQueryable<TEntity> query, SearchRequest<TParameters> searchRequest)
		where TParameters : class, ISearchParameters
	{
		var direction = searchRequest.OrderDirection ?? OrderDirection.Asc;
		IOrderedQueryable<TEntity> addOrderDirection(Expression<Func<TEntity, string?>> keySelector)
			=> direction == OrderDirection.Asc ? query.OrderBy(keySelector) : query.OrderByDescending(keySelector);

		var property = searchRequest.OrderProperty?.ToLower();
		query = (property) switch
		{
			_ => addOrderDirection(x => x.Id.ToString()),
		};

		return query;
	}

	/// <summary>
	/// This method is used to add paging to the search query.
	/// </summary>
	public virtual IQueryable<TEntity> AddPaging<TParameters>(IQueryable<TEntity> query, SearchRequest<TParameters> searchRequest)
		where TParameters : class, ISearchParameters
	{
		var page = searchRequest.GetPage();
		var pageSize = searchRequest.GetPageSize();

		query = query.Skip((page - 1) * pageSize);
		query = query.Take(pageSize);

		return query;
	}

	/// <summary>
	/// This method is used to add filters to the search query. This method is not implemented by default.
	/// </summary>
	public virtual IQueryable<TEntity> AddFilters<TParameters>(IQueryable<TEntity> query, SearchRequest<TParameters> searchRequest)
		where TParameters : class, ISearchParameters
	{
		throw new NotImplementedException();
	}

	#endregion
}
