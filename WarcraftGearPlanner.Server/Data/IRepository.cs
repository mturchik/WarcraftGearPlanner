using System.Linq.Expressions;
using WarcraftGearPlanner.Server.Data.Entities;
using WarcraftGearPlanner.Shared.Requests.Search;

namespace WarcraftGearPlanner.Server.Data;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
	Task<TEntity> CreateAsync(TEntity entity);
	Task<List<TEntity>> CreateListAsync(List<TEntity> entities);
	Task DeleteAsync(TEntity entity);
	Task DeleteListAsync(List<TEntity> entities);
	Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> selector);
	Task<TEntity?> GetByIdAsync(Guid id);
	Task<int> GetCountAsync();
	Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? selector = null);
	Task<(List<TEntity> entities, int count)> SearchAsync<TParameters>(SearchRequest<TParameters> searchRequest) where TParameters : class, ISearchParameters;
	Task<TEntity> UpdateAsync(TEntity entity);
	Task<List<TEntity>> UpdateListAsync(List<TEntity> entities);
}
