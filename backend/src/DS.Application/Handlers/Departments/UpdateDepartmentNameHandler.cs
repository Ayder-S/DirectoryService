using CSharpFunctionalExtensions;
using DS.Application.Commands.Department;
using DS.Application.Interfaces.Abstractions;
using DS.Application.Interfaces.Database;
using DS.Application.Validation;
using DS.Contracts.Departments.Update;
using DS.Domain.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.Kernel.AppFails;

namespace DS.Application.Handlers.Departments;

public class UpdateDepartmentNameHandler : ICommandHandler<Guid, UpdateDepartmentNameCommand>
{
    private readonly IDepartmentsRepository _departmentsRepository;
    private readonly IValidator<UpdateDepartmentNameRequest> _validator;
    private readonly ILogger<UpdateDepartmentNameHandler> _logger;
    
    public UpdateDepartmentNameHandler(IDepartmentsRepository departmentsRepository, IValidator<UpdateDepartmentNameRequest> validator, ILogger<UpdateDepartmentNameHandler> logger)
    {
        _departmentsRepository = departmentsRepository;
        _validator = validator;
        _logger = logger;
    }
    
    public async Task<Result<Guid, ErrorsList>> Handle(UpdateDepartmentNameCommand command, CancellationToken cancellationToken)
    {
        if (command.Request is null)
            return Error.Validation("request.is.required", "Тело запроса обязательно").ToErrors();
        
        var validationResult = await _validator.ValidateAsync(command.Request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrors();
        
        var departmentResult = await _departmentsRepository.GetById(command.Id, cancellationToken);
        if (departmentResult.IsFailure)
            return departmentResult.Error.ToErrors();
        
        var name = Name.Create(command.Request.Name).Value;
        
        var department = departmentResult.Value;
        
        department.UpdateDepartment(name);

        var updateResult = await _departmentsRepository.UpdateName(department, cancellationToken);
        if (updateResult.IsFailure)
            return updateResult.Error.ToErrors();
        
        _logger.LogInformation("Подразделение {DepartmentId} успешно обновлено", department.Id);

        return command.Id;
    }
}