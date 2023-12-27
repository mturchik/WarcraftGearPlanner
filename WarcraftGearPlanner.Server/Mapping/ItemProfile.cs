using AutoMapper;
using WarcraftGearPlanner.Server.Data.Entities;
using WarcraftGearPlanner.Shared.Models;
using WarcraftGearPlanner.Shared.Models.Items;

namespace WarcraftGearPlanner.Server.Mapping;

public class ItemProfile : Profile
{
	public ItemProfile()
	{
		CreateMap<ItemEntity, Item>().IncludeBase<BaseEntity, BaseModel>()
			.ForMember(x => x.ItemClassName, opt =>
			{
				opt.PreCondition(src => src.ItemClass != null);
				opt.MapFrom(src => src.ItemClass!.Name);
			})
			.ForMember(x => x.ItemClassClassId, opt =>
			{
				opt.PreCondition(src => src.ItemClass != null);
				opt.MapFrom(src => src.ItemClass!.ClassId);
			})
			.ForMember(x => x.ItemSubclassName, opt =>
			{
				opt.PreCondition(src => src.ItemSubclass != null);
				opt.MapFrom(src => src.ItemSubclass!.Name);
			})
			.ForMember(x => x.ItemSubclassSubclassId, opt =>
			{
				opt.PreCondition(src => src.ItemSubclass != null);
				opt.MapFrom(src => src.ItemSubclass!.SubclassId);
			})
			.ForMember(x => x.ItemQualityName, opt =>
			{
				opt.PreCondition(src => src.ItemQuality != null);
				opt.MapFrom(src => src.ItemQuality!.Name);
			})
			.ForMember(x => x.ItemQualityType, opt =>
			{
				opt.PreCondition(src => src.ItemQuality != null);
				opt.MapFrom(src => src.ItemQuality!.Type);
			})
			.ForMember(x => x.InventoryTypeName, opt =>
			{
				opt.PreCondition(src => src.InventoryType != null);
				opt.MapFrom(src => src.InventoryType!.Name);
			})
			.ForMember(x => x.InventoryTypeType, opt =>
			{
				opt.PreCondition(src => src.InventoryType != null);
				opt.MapFrom(src => src.InventoryType!.Type);
			});

		CreateMap<Item, ItemEntity>().IncludeBase<BaseModel, BaseEntity>()
			.ForMember(x => x.ItemClass, opt => opt.Ignore())
			.ForMember(x => x.ItemSubclass, opt => opt.Ignore())
			.ForMember(x => x.ItemQuality, opt => opt.Ignore())
			.ForMember(x => x.InventoryType, opt => opt.Ignore());
	}
}
