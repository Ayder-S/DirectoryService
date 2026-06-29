using CSharpFunctionalExtensions;
using DS.Application.Interfaces.Abstractions;
using DS.Application.Interfaces.Database;
using DS.Application.Validation;
using DS.Contracts.Departments.Create;
using DS.Domain.Entities;
using DS.Domain.Relation;
using DS.Domain.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.Kernel.AppFails;

namespace DS.Application.Features.Departments.CreateDepartment;

public class CreateDepartmentHandler : ICommandHandler<Guid, CreateDepartmentCommand>
{
    private readonly IDepartmentsRepository _departmentsRepository;
    private readonly ILocationsRepository _locationsRepository;
    private readonly IValidator<CreateDepartmentRequest> _validator;
    private readonly ILogger<CreateDepartmentHandler> _logger;
    
    public CreateDepartmentHandler(
        IDepartmentsRepository departmentsRepository, 
        IValidator<CreateDepartmentRequest> validator, 
        ILogger<CreateDepartmentHandler> logger, 
        ILocationsRepository locationsRepository)
    {
        _departmentsRepository = departmentsRepository;
        _locationsRepository = locationsRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorsList>> Handle(CreateDepartmentCommand command, CancellationToken cancellationToken)
    {
        if (command.Request is null)
            return Error.Validation("request.is.required", "Тело запроса обязательно").ToErrors();
        
        var validationResult = await _validator.ValidateAsync(command.Request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrors();
        
        var name = Name.Create(command.Request.Name).Value;
        var identifier = Identifier.Create(command.Request.Identifier).Value;
        
        Department? parent = null;
        if (command.Request.ParentId != null)
        {
            Guid parentId = command.Request.ParentId.Value;
            var parentResult = await _departmentsRepository.GetById(parentId, cancellationToken);
            if (parentResult.IsFailure)
                return parentResult.Error.ToErrors();
            
            parent = parentResult.Value;
        }

        if (command.Request.LocationIds.Count > 0 &&
            !await _locationsRepository.AllExist(command.Request.LocationIds, cancellationToken))
            return Error.NotFound("locations.not_found", "Одна или несколько локаций не найдены").ToErrors();

        var departmentResult = Department.Create(name, identifier, parent);
        if (departmentResult.IsFailure)
            return departmentResult.Error.ToErrors();

        var department = departmentResult.Value;

        var relations = command.Request.LocationIds
            .Select(locationId => DepartmentLocation.Create(department.Id, locationId)).ToList();
        
        var addResult = await _departmentsRepository.Add(department, relations, cancellationToken);
        if (addResult.IsFailure)
            return addResult.Error.ToErrors();
        
        _logger.LogInformation("Подразделение {DepartmentId} успешно создано с названием {DepartmentName}", department.Id, department.Name.Value);

        return department.Id;
    }
}