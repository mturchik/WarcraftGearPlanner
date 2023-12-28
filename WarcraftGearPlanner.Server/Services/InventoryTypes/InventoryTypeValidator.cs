using FluentValidation;
using WarcraftGearPlanner.Shared.Models.Items;

namespace WarcraftGearPlanner.Server.Services.InventoryTypes;

public class InventoryTypeValidator : AbstractValidator<InventoryType>
{
	public InventoryTypeValidator()
	{

	}
}
