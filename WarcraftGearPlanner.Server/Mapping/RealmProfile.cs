using AutoMapper;
using WarcraftGearPlanner.Server.Data.Entities;
using WarcraftGearPlanner.Shared.Models;
using WarcraftGearPlanner.Shared.Models.Realms;

namespace WarcraftGearPlanner.Server.Mapping;

public class RealmProfile : Profile
{
	public RealmProfile()
	{
		CreateMap<RealmEntity, Realm>().IncludeBase<BaseEntity, BaseModel>();
		CreateMap<Realm, RealmEntity>().IncludeBase<BaseModel, BaseEntity>();
	}
}
