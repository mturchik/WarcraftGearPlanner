using System.Linq.Expressions;
using WarcraftGearPlanner.Server.Data.Entities;

namespace WarcraftGearPlanner.Server.Data;

public interface IRepository<T> where T : BaseEntity
{
	Task<T> CreateAsync(T entity);
	Task DeleteAsync(Guid id);
	Task<T?> GetAsync(Expression<Func<T, bool>> selector);
	Task<T?> GetByIdAsync(Guid id);
	Task<List<T>> GetListAsync(Expression<Func<T, bool>>? selector = null);
	Task<T> UpdateAsync(T entity);
}
