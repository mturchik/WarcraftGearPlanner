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
		var createdModel = mapper.Map<TModel>(createdEntity);

		memoryCache.Remove(cacheKey + "_GetList");
		memoryCache.Set(cacheKey + "_GetById_" + createdModel.Id, createdModel);

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
		var createdModels = mapper.Map<List<TModel>>(createdEntities);

		memoryCache.Remove(cacheKey + "_GetList");
		foreach (var createdModel in createdModels)
			memoryCache.Set(cacheKey + "_GetById_" + createdModel.Id, createdModel);

		return createdModels;
	}

	public async Task DeleteAsync(Guid id)
	{
		var entity = await repository.GetByIdAsync(id);

		if (entity is not null)
			await repository.DeleteAsync(entity);

		memoryCache.Remove(cacheKey + "_GetList");
		memoryCache.Remove(cacheKey + "_GetById_" + id);
	}

	public async Task DeleteListAsync(List<Guid> ids)
	{
		var entities = await repository.GetListAsync(e => ids.Contains(e.Id));

		if (entities.Count > 0)
			await repository.DeleteListAsync(entities);

		memoryCache.Remove(cacheKey + "_GetList");
		foreach (var id in ids)
			memoryCache.Remove(cacheKey + "_GetById_" + id);
	}

	public async Task<TModel?> GetAsync(Expression<Func<TEntity, bool>> selector)
	{
		var entity = await repository.GetAsync(selector);
		var model = mapper.Map<TModel>(entity);
		return model;
	}

	public async Task<TModel?> GetByIdAsync(Guid id)
	{
		var entity = await memoryCache.GetOrCreateAsync(cacheKey + "_GetById_" + id, entry => repository.GetByIdAsync(id));
		var model = mapper.Map<TModel>(entity);
		return model;
	}

	public async Task<List<TModel>> GetListAsync(Expression<Func<TEntity, bool>>? selector = null)
	{
		var entities = selector is null
			? await memoryCache.GetOrCreateAsync(cacheKey + "_GetList", entry => repository.GetListAsync())
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
		var updatedModel = mapper.Map<TModel>(entity);

		memoryCache.Remove(cacheKey + "_GetList");
		memoryCache.Set(cacheKey + "_GetById_" + model.Id, updatedModel);

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
		var updatedModels = mapper.Map<List<TModel>>(entities);

		memoryCache.Remove(cacheKey + "_GetList");
		foreach (var updatedModel in updatedModels)
			memoryCache.Set(cacheKey + "_GetById_" + updatedModel.Id, updatedModel);

		return updatedModels;
	}
}
