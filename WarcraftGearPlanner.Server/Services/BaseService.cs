using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using WarcraftGearPlanner.Server.Data;
using WarcraftGearPlanner.Server.Data.Entities;
using WarcraftGearPlanner.Shared.Models;

namespace WarcraftGearPlanner.Server.Services;

public abstract class BaseService<TModel, TEntity>(
	IRepository<TEntity> repository,
	IValidator<TModel> validator,
	IMemoryCache memoryCache,
	IMapper mapper
) : IService<TModel, TEntity>
	where TModel : BaseModel
	where TEntity : BaseEntity
{
	protected readonly IRepository<TEntity> repository = repository;
	protected readonly IValidator<TModel> validator = validator;
	protected readonly IMemoryCache memoryCache = memoryCache;
	protected readonly IMapper mapper = mapper;
	protected readonly string cacheKey = typeof(TEntity).Name;

	public virtual async Task<TModel> CreateAsync(TModel model)
	{
		await validator.ValidateAndThrowAsync(model);

		var entity = await repository.GetByIdAsync(model.Id);
		if (entity is not null)
			throw new Exception($"Entity with id {model.Id} already exists");

		entity = mapper.Map<TEntity>(model);
		var createdEntity = await repository.CreateAsync(entity);

		memoryCache.Remove(cacheKey);

		var createdModel = mapper.Map<TModel>(createdEntity);
		return createdModel;
	}

	public virtual async Task<List<TModel>> CreateListAsync(List<TModel> models)
	{
		foreach (var model in models)
		{
			await validator.ValidateAndThrowAsync(model);

			var entity = await repository.GetByIdAsync(model.Id);
			if (entity is not null)
				throw new Exception($"Entity with id {model.Id} already exists");
		}

		var entities = mapper.Map<List<TEntity>>(models);
		var createdEntities = await repository.CreateListAsync(entities);

		memoryCache.Remove(cacheKey);

		var createdModels = mapper.Map<List<TModel>>(createdEntities);
		return createdModels;
	}

	public virtual async Task DeleteAsync(Guid id)
	{
		var entity = await repository.GetByIdAsync(id);
		if (entity is null) return;

		await repository.DeleteAsync(entity);
		memoryCache.Remove(cacheKey);
	}

	public virtual async Task DeleteListAsync(List<Guid> ids)
	{
		var entities = await repository.GetListAsync(e => ids.Contains(e.Id));
		if (entities.Count <= 0) return;

		await repository.DeleteListAsync(entities);
		memoryCache.Remove(cacheKey);
	}

	public virtual async Task<TModel?> GetByIdAsync(Guid id)
	{
		var entity = await repository.GetByIdAsync(id);
		if (entity is null) return null;

		var model = mapper.Map<TModel>(entity);
		return model;
	}

	public virtual Task<int> GetCountAsync() => repository.GetCountAsync();

	public virtual async Task<List<TModel>> GetListAsync()
	{
		var entities = memoryCache.Get<List<TEntity>>(cacheKey);
		if (entities is null)
		{
			entities = await repository.GetListAsync();
			memoryCache.Set(cacheKey, entities);
		}

		var models = mapper.Map<List<TModel>>(entities);
		return models;
	}

	public virtual async Task<TModel> UpdateAsync(TModel model)
	{
		await validator.ValidateAndThrowAsync(model);

		var entity = await repository.GetByIdAsync(model.Id)
			?? throw new Exception($"Entity with id {model.Id} not found");

		mapper.Map(model, entity);
		entity = await repository.UpdateAsync(entity);

		memoryCache.Remove(cacheKey);

		var updatedModel = mapper.Map<TModel>(entity);
		return updatedModel;
	}

	public virtual async Task<List<TModel>> UpdateListAsync(List<TModel> models)
	{
		foreach (var model in models)
			await validator.ValidateAndThrowAsync(model);

		var entities = await repository.GetListAsync(e => models.Select(m => m.Id).Contains(e.Id));
		if (entities.Count != models.Count)
			throw new Exception("Not all entities were found");

		foreach (var entity in entities)
		{
			var model = models.First(m => m.Id == entity.Id);
			mapper.Map(model, entity);
		}
		entities = await repository.UpdateListAsync(entities);

		memoryCache.Remove(cacheKey);

		var updatedModels = mapper.Map<List<TModel>>(entities);
		return updatedModels;
	}
}
