using AutoMapper;
using WarcraftGearPlanner.Server.Data.Entities;
using WarcraftGearPlanner.Shared.Models;
using WarcraftGearPlanner.Shared.Models.Items;

namespace WarcraftGearPlanner.Server.Mapping;

public class InventoryTypeProfile : Profile
{
	public InventoryTypeProfile()
	{
		CreateMap<InventoryTypeEntity, InventoryType>().IncludeBase<BaseEntity, BaseModel>();
		CreateMap<InventoryType, InventoryTypeEntity>().IncludeBase<BaseModel, BaseEntity>()
			.ForMember(e => e.Items, opt => opt.Ignore());
	}
}
