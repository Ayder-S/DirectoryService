using DS.Contracts.Locations.Create;
using DS.Domain.ValueObjects;
using FluentValidation;

namespace DS.Application.Validation.LocationValidator;

public class CreateLocationRequestValidator : AbstractValidator<CreateLocationRequest>
{
    public CreateLocationRequestValidator()
    {
        RuleFor(l => l.Name)
            .MustBeValueObject(Name.Create);

        RuleFor(l => l.Address)
            .MustBeValueObject(a => Address.Create(
                a.Country,
                a.Region,
                a.City,
                a.Street,
                a.Building));
        
        RuleFor(l => l.Timezone)
            .MustBeValueObject(Timezone.Create);
    }
}