using DS.Contracts.Departments.Update;
using DS.Domain.ValueObjects;
using FluentValidation;

namespace DS.Application.Validation.DepartmentValidator;

public class UpdateDepartmentNameRequestValidator : AbstractValidator<UpdateDepartmentNameRequest>
{
    public UpdateDepartmentNameRequestValidator()
    {
        RuleFor(d => d.Name).MustBeValueObject(Name.Create);
    }
}