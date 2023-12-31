using AutoMapper;
using WarcraftGearPlanner.Server.Data.Entities;
using WarcraftGearPlanner.Shared.Models;
using WarcraftGearPlanner.Shared.Models.Items;

namespace WarcraftGearPlanner.Server.Mapping;

public class ItemSubclassProfile : Profile
{
	public ItemSubclassProfile()
	{
		CreateMap<ItemSubclassEntity, ItemSubclass>().IncludeBase<BaseEntity, BaseModel>()
			.ForMember(m => m.InventoryTypes, opt =>
			{
				opt.Condition(e => e.ItemSubclassInventoryTypes != null);
				opt.MapFrom(e => e.ItemSubclassInventoryTypes!.Select(i => i.InventoryType));
			});
		CreateMap<ItemSubclass, ItemSubclassEntity>().IncludeBase<BaseModel, BaseEntity>()
			.ForMember(e => e.ItemClassId, opt => opt.Ignore())
			.ForMember(e => e.ItemClass, opt => opt.Ignore())
			.ForMember(e => e.Items, opt => opt.Ignore())
			.ForMember(e => e.ItemSubclassInventoryTypes, opt => opt.Ignore());
	}
}
