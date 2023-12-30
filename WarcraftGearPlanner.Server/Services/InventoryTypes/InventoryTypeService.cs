using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using WarcraftGearPlanner.Server.Data;
using WarcraftGearPlanner.Server.Data.Entities;
using WarcraftGearPlanner.Shared.Models.Items;

namespace WarcraftGearPlanner.Server.Services.InventoryTypes;

public class InventoryTypeService(
	IRepository<InventoryTypeEntity> repository,
	IValidator<InventoryType> validator,
	IMemoryCache memoryCache,
	IMapper mapper
) : BaseService<InventoryType, InventoryTypeEntity>(repository, validator, memoryCache, mapper), IInventoryTypeService
{
}
