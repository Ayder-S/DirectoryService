using DS.Application.Validation;
using DS.Contracts.Locations.Create;
using DS.Domain.ValueObjects;
using FluentValidation;
using Shared.Constants;

namespace DS.Application.Validators.LocationValidator;

public class CreateLocationRequestValidator : AbstractValidator<CreateLocationRequest>
{
    public CreateLocationRequestValidator()
    {
        RuleFor(l => l.Name)
            .MustBeValueObject(Name.Create);

        // RuleFor(l => l.Address)
        //     .MustBeValueObject(Address.Create());
    }
}