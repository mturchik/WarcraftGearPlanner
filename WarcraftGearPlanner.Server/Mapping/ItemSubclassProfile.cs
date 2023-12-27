using AutoMapper;
using WarcraftGearPlanner.Server.Data.Entities;
using WarcraftGearPlanner.Shared.Models;
using WarcraftGearPlanner.Shared.Models.Items;

namespace WarcraftGearPlanner.Server.Mapping;

public class ItemSubclassProfile : Profile
{
	public ItemSubclassProfile()
	{
		CreateMap<ItemSubclassEntity, ItemSubclass>().IncludeBase<BaseEntity, BaseModel>();
		CreateMap<ItemSubclass, ItemSubclassEntity>().IncludeBase<BaseModel, BaseEntity>()
			.ForMember(e => e.ItemClassId, opt => opt.Ignore())
			.ForMember(e => e.ItemClass, opt => opt.Ignore())
			.ForMember(e => e.Items, opt => opt.Ignore());
	}
}
