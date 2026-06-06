using DS.Contracts.Locations.Update;
using DS.Domain.ValueObjects;
using FluentValidation;

namespace DS.Application.Validation.LocationValidator;

public class UpdateLocationRequestValidator : AbstractValidator<UpdateLocationRequest>
{
    public UpdateLocationRequestValidator()
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
        
        RuleFor(l => l.TimeZone)
            .MustBeValueObject(Timezone.Create);
    }
}