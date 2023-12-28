using FluentValidation;
using WarcraftGearPlanner.Shared.Models.Items;

namespace WarcraftGearPlanner.Server.Services.ItemQualities;

public class ItemQualityValidator : AbstractValidator<ItemQuality>
{
	public ItemQualityValidator()
	{

	}
}
