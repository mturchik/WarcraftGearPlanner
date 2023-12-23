using FluentValidation;
using WarcraftGearPlanner.Shared.Models.Realms;

namespace WarcraftGearPlanner.Server.Services.Realms;

public class RealmValidator : AbstractValidator<Realm>
{
	public RealmValidator()
	{
		RuleFor(x => x.Id).NotEmpty();
		RuleFor(x => x.RealmId).NotEmpty();
		RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
		RuleFor(x => x.Slug).NotEmpty().MaximumLength(50);
	}
}
