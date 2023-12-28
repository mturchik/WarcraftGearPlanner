using AutoMapper;
using WarcraftGearPlanner.Server.Data.Entities;
using WarcraftGearPlanner.Shared.Models;
using WarcraftGearPlanner.Shared.Models.Items;

namespace WarcraftGearPlanner.Server.Mapping;

public class ItemQualityProfile : Profile
{
	public ItemQualityProfile()
	{
		CreateMap<ItemQualityEntity, ItemQuality>().IncludeBase<BaseEntity, BaseModel>();
		CreateMap<ItemQuality, ItemQualityEntity>().IncludeBase<BaseModel, BaseEntity>()
			.ForMember(e => e.Items, opt => opt.Ignore());
	}
}
