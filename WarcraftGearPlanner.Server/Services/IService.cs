using System.Linq.Expressions;
using WarcraftGearPlanner.Server.Data.Entities;
using WarcraftGearPlanner.Shared.Models;

namespace WarcraftGearPlanner.Server.Services;

public interface IService<TModel, TEntity>
	where TModel : BaseModel
	where TEntity : BaseEntity
{
	Task<TModel> CreateAsync(TModel model);
	Task<List<TModel>> CreateListAsync(List<TModel> models);
	Task DeleteAsync(Guid id);
	Task DeleteListAsync(List<Guid> ids);
	Task<TModel?> GetAsync(Expression<Func<TEntity, bool>> selector);
	Task<TModel?> GetByIdAsync(Guid id);
	Task<List<TModel>> GetListAsync(Expression<Func<TEntity, bool>>? selector = null);
	Task<TModel> UpdateAsync(TModel model);
	Task<List<TModel>> UpdateListAsync(List<TModel> models);
}
