using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using WarcraftGearPlanner.Server.Data;
using WarcraftGearPlanner.Server.Data.Entities;
using WarcraftGearPlanner.Shared.Models.Realms;

namespace WarcraftGearPlanner.Server.Services.Realms;

public class RealmService(
	IRepository<RealmEntity> repository,
	IValidator<Realm> validator,
	IMemoryCache memoryCache,
	IMapper mapper
) : BaseService<Realm, RealmEntity>(repository, validator, memoryCache, mapper), IRealmService
{
}
