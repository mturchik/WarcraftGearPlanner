using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WarcraftGearPlanner.Server.Data;
using WarcraftGearPlanner.Server.Data.Entities;
using WarcraftGearPlanner.Shared.Models.Items;

namespace WarcraftGearPlanner.Server.Services.ItemClasses;

public class ItemClassService(
    IRepository<ItemClassEntity> repository,
    IValidator<ItemClass> validator,
    IMemoryCache memoryCache,
    IMapper mapper
) : BaseService<ItemClass, ItemClassEntity>(repository, validator, memoryCache, mapper), IItemClassService
{
    public override async Task<ItemClass> CreateAsync(ItemClass model)
    {
        await validator.ValidateAndThrowAsync(model);

        var entity = await repository.GetByIdAsync(model.Id);
        if (entity is not null) throw new Exception($"Entity with id {model.Id} already exists");

        entity = mapper.Map<ItemClassEntity>(model);
        MapSubclasses(model, entity);
        var createdEntity = await repository.CreateAsync(entity);

        memoryCache.Remove(cacheKey);

        var createdModel = mapper.Map<ItemClass>(createdEntity);
        return createdModel;
    }

    public override async Task<List<ItemClass>> CreateListAsync(List<ItemClass> models)
    {
        foreach (var model in models)
        {
            await validator.ValidateAndThrowAsync(model);

            var entity = await repository.GetByIdAsync(model.Id);
            if (entity is not null)
                throw new Exception($"Entity with id {model.Id} already exists");
        }

        var entities = mapper.Map<List<ItemClassEntity>>(models);
        foreach (var entity in entities)
            MapSubclasses(models.First(m => m.Id == entity.Id), entity);
        var createdEntities = await repository.CreateListAsync(entities);

        memoryCache.Remove(cacheKey);

        var createdModels = mapper.Map<List<ItemClass>>(createdEntities);
        return createdModels;
    }

    public override async Task<ItemClass> UpdateAsync(ItemClass model)
    {
        await validator.ValidateAndThrowAsync(model);

        var entity = await repository.GetByIdAsync(model.Id)
            ?? throw new Exception($"Entity with id {model.Id} not found");

        ValidateSubclassUpdate(model, entity);

        mapper.Map(model, entity);
        MapSubclasses(model, entity);
        entity = await repository.UpdateAsync(entity);

        memoryCache.Remove(cacheKey);

        var updatedModel = mapper.Map<ItemClass>(entity);
        return updatedModel;
    }

    public override async Task<List<ItemClass>> UpdateListAsync(List<ItemClass> models)
    {
        foreach (var model in models)
            await validator.ValidateAndThrowAsync(model);

        var entities = await repository.GetListAsync(e => models.Select(m => m.Id).Contains(e.Id));
        if (entities.Count != models.Count)
            throw new Exception("Not all entities were found");

        foreach (var entity in entities)
        {
            var model = models.First(m => m.Id == entity.Id);

            ValidateSubclassUpdate(model, entity);

            mapper.Map(model, entity);
            MapSubclasses(model, entity);
        }
        entities = await repository.UpdateListAsync(entities);

        memoryCache.Remove(cacheKey);

        var updatedModels = mapper.Map<List<ItemClass>>(entities);
        return updatedModels;
    }

    private static void ValidateSubclassUpdate(ItemClass model, ItemClassEntity entity)
    {
        model.Subclasses?.ForEach(s =>
        {
            var e = entity.Subclasses?.FirstOrDefault(e => e.Id == s.Id);
            if (s.Id != Guid.Empty && e is null)
                throw new Exception($"Subclass with id {s.Id} not found");
        });
    }

    private void MapSubclasses(ItemClass model, ItemClassEntity entity)
    {
        model.Subclasses ??= [];
        entity.Subclasses ??= [];
        model.Subclasses.ForEach(subclass =>
        {
            var subclassEntity = entity.Subclasses.FirstOrDefault(e => e.Id == subclass.Id && e.Id != Guid.Empty);
            if (subclassEntity is null)
            {
                subclassEntity = mapper.Map<ItemSubclassEntity>(subclass);
                entity.Subclasses.Add(subclassEntity);
            }
            else mapper.Map(subclass, subclassEntity);
        });
        entity.Subclasses.RemoveAll(e => e.Id != Guid.Empty && model.Subclasses.All(m => m.Id != e.Id));
    }
}
