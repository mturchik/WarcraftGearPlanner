using AutoMapper;
using WarcraftGearPlanner.Server.Data.Entities;
using WarcraftGearPlanner.Shared.Models;
using WarcraftGearPlanner.Shared.Models.Items;

namespace WarcraftGearPlanner.Server.Mapping;

public class ItemClassProfile : Profile
{
	public ItemClassProfile()
	{
		CreateMap<ItemClassEntity, ItemClass>().IncludeBase<BaseEntity, BaseModel>();
		CreateMap<ItemClass, ItemClassEntity>().IncludeBase<BaseModel, BaseEntity>()
			.ForMember(e => e.Subclasses, opt => opt.Ignore())
			.ForMember(e => e.Items, opt => opt.Ignore());
	}
}
