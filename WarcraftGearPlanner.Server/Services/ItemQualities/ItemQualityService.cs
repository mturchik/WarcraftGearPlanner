using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using WarcraftGearPlanner.Server.Data;
using WarcraftGearPlanner.Server.Data.Entities;
using WarcraftGearPlanner.Shared.Models.Items;

namespace WarcraftGearPlanner.Server.Services.ItemQualities;

public class ItemQualityService(
	IRepository<ItemQualityEntity> repository,
	IValidator<ItemQuality> validator,
	IMemoryCache memoryCache,
	IMapper mapper
) : BaseService<ItemQuality, ItemQualityEntity>(repository, validator, memoryCache, mapper), IItemQualityService
{
}
