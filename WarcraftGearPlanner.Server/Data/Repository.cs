using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WarcraftGearPlanner.Server.Data.Entities;

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
		var entity = await TableQuery.FirstOrDefaultAsync(x => x.Id == id);
		return entity;
	}

	public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? selector = null)
	{
		var query = TableQuery;
		if (selector != null) query = query.Where(selector);

		var entities = await query.ToListAsync();
		return entities;
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
}
