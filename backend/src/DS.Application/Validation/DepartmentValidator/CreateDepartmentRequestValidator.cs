using DS.Contracts.Departments.Create;
using DS.Domain.ValueObjects;
using FluentValidation;

namespace DS.Application.Validation.DepartmentValidator;

public class CreateDepartmentRequestValidator : AbstractValidator<CreateDepartmentRequest>
{
    public CreateDepartmentRequestValidator()
    {
        RuleFor(d => d.Name)
            .MustBeValueObject(Name.Create);
        
        RuleFor(d => d.Identifier)
            .MustBeValueObject(Identifier.Create);
    }
}