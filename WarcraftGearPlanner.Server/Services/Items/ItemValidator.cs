using FluentValidation;
using WarcraftGearPlanner.Shared.Models.Items;

namespace WarcraftGearPlanner.Server.Services.Items;

public class ItemValidator : AbstractValidator<Item>
{
	public ItemValidator()
	{
		RuleFor(x => x.ItemId).NotEmpty();
		RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
		RuleFor(x => x.ItemClassId).NotEmpty();
		RuleFor(x => x.ItemSubclassId).NotEmpty();
	}
}
