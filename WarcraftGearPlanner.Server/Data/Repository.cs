using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WarcraftGearPlanner.Server.Data.Entities;

namespace WarcraftGearPlanner.Server.Data;

public abstract class Repository<T>(ApplicationDbContext context) : IRepository<T> where T : BaseEntity
{
	protected readonly ApplicationDbContext _context = context;

	protected DbSet<T> Table => _context.Set<T>();
	protected IQueryable<T> TableQuery => Table.AsQueryable();

	public async Task<T> CreateAsync(T entity)
	{
		await Table.AddAsync(entity);
		await _context.SaveChangesAsync();
		return entity;
	}

	public async Task DeleteAsync(Guid id)
	{
		var entity = await TableQuery.FirstOrDefaultAsync(x => x.Id == id);
		if (entity == null) return;

		Table.Remove(entity);
		await _context.SaveChangesAsync();
	}

	public virtual async Task<T?> GetAsync(Expression<Func<T, bool>> selector)
	{
		var entity = await TableQuery.FirstOrDefaultAsync(selector);
		return entity;
	}

	public virtual async Task<T?> GetByIdAsync(Guid id)
	{
		var entity = await TableQuery.FirstOrDefaultAsync(x => x.Id == id);
		return entity;
	}

	public virtual async Task<List<T>> GetListAsync(Expression<Func<T, bool>>? selector = null)
	{
		var query = TableQuery;
		if (selector != null) query = query.Where(selector);
		var entities = await query.ToListAsync();
		return entities;
	}

	public async Task<T> UpdateAsync(T entity)
	{
		Table.Update(entity);
		await _context.SaveChangesAsync();
		return entity;
	}
}
