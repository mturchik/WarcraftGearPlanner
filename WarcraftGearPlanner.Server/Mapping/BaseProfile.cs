using AutoMapper;
using WarcraftGearPlanner.Server.Data.Entities;
using WarcraftGearPlanner.Shared.Models;

namespace WarcraftGearPlanner.Server.Mapping;

public class BaseProfile : Profile
{
	public BaseProfile()
	{
		CreateMap<BaseEntity, BaseModel>();
		CreateMap<BaseModel, BaseEntity>()
			.ForMember(x => x.CreatedAt, opt => opt.Ignore());
	}
}
