using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;
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

	public async Task<TModel> CreateAsync(TModel model)
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

	public async Task<List<TModel>> CreateListAsync(List<TModel> models)
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

	public async Task DeleteAsync(Guid id)
	{
		var entity = await repository.GetByIdAsync(id);

		if (entity is not null)
			await repository.DeleteAsync(entity);

		memoryCache.Remove(cacheKey);
	}

	public async Task DeleteListAsync(List<Guid> ids)
	{
		var entities = await repository.GetListAsync(e => ids.Contains(e.Id));

		if (entities.Count > 0)
			await repository.DeleteListAsync(entities);

		memoryCache.Remove(cacheKey);
	}

	public async Task<TModel?> GetAsync(Expression<Func<TEntity, bool>> selector)
	{
		var entity = await repository.GetAsync(selector);
		var model = mapper.Map<TModel>(entity);
		return model;
	}

	public async Task<TModel?> GetByIdAsync(Guid id)
	{
		var entity = await repository.GetByIdAsync(id);
		var model = mapper.Map<TModel>(entity);
		return model;
	}

	public async Task<List<TModel>> GetListAsync(Expression<Func<TEntity, bool>>? selector = null)
	{
		var entities = selector is null
			? await memoryCache.GetOrCreateAsync(cacheKey, entry => repository.GetListAsync())
			: await repository.GetListAsync(selector);
		var models = mapper.Map<List<TModel>>(entities ?? []);
		return models;
	}

	public async Task<TModel> UpdateAsync(TModel model)
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

	public async Task<List<TModel>> UpdateListAsync(List<TModel> models)
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
