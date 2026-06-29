using DS.Application.Validation;
using DS.Contracts.Departments.Update;
using DS.Domain.ValueObjects;
using FluentValidation;

namespace DS.Application.Features.Departments.UpdateDepartment;

public class UpdateDepartmentNameRequestValidator : AbstractValidator<UpdateDepartmentNameRequest>
{
    public UpdateDepartmentNameRequestValidator()
    {
        RuleFor(d => d.Name).MustBeValueObject(Name.Create);
    }
}