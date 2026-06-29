using DS.Application.Validation;
using DS.Contracts.Departments.Create;
using DS.Domain.ValueObjects;
using FluentValidation;

namespace DS.Application.Features.Departments.CreateDepartment;

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