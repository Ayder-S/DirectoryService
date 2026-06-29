using DS.Application.Validation;
using DS.Contracts.Locations.Update;
using DS.Domain.ValueObjects;
using FluentValidation;

namespace DS.Application.Features.Locations.UpdateLocationName;

public class UpdateLocationNameRequestValidator : AbstractValidator<UpdateLocationNameRequest>
{
    public UpdateLocationNameRequestValidator()
    {
        RuleFor(l => l.Name).MustBeValueObject(Name.Create);
    }
}