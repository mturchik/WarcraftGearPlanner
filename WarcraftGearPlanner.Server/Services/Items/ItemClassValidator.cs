using FluentValidation;
using WarcraftGearPlanner.Shared.Models.Items;

namespace WarcraftGearPlanner.Server.Services.Items;

public class ItemClassValidator : AbstractValidator<ItemClass>
{
	public ItemClassValidator()
	{
		RuleFor(x => x.ClassId).NotEmpty();
		RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
		RuleForEach(x => x.Subclasses).ChildRules(subclass =>
		{
			subclass.RuleFor(x => x.SubclassId).NotEmpty();
			subclass.RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
		});
	}
}
