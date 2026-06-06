using DS.Contracts.Locations.Update;
using DS.Domain.ValueObjects;
using FluentValidation;

namespace DS.Application.Validation.LocationValidator;

public class UpdateLocationNameRequestValidator : AbstractValidator<UpdateLocationNameRequest>
{
    public UpdateLocationNameRequestValidator()
    {
        RuleFor(l => l.Name).MustBeValueObject(Name.Create);
    }
}