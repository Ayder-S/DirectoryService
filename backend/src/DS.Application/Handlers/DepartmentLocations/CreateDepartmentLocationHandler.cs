using CSharpFunctionalExtensions;
using DS.Application.Commands.DepartmentLocation;
using DS.Application.Interfaces.Abstractions;
using DS.Application.Interfaces.Database;
using Microsoft.Extensions.Logging;
using Shared.Kernel.AppFails;

namespace DS.Application.Handlers.DepartmentLocations;

public class CreateDepartmentLocationHandler : ICommandHandler<CreateDepartmentLocationCommand>
{
    private readonly IDepartmentsRepository _departmentsRepository;
    private readonly ILocationsRepository _locationsRepository;
    private readonly IDepartmentLocationsRepository _departmentLocationsRepository;
    private readonly ILogger<CreateDepartmentLocationHandler> _logger;

    public CreateDepartmentLocationHandler(
        IDepartmentsRepository departmentsRepository, 
        ILocationsRepository locationsRepository, 
        IDepartmentLocationsRepository departmentLocationsRepository, 
        ILogger<CreateDepartmentLocationHandler> logger)
    {
        _departmentsRepository = departmentsRepository;
        _locationsRepository = locationsRepository;
        _departmentLocationsRepository = departmentLocationsRepository;
        _logger = logger;
    }

    public async Task<UnitResult<ErrorsList>> Handle(CreateDepartmentLocationCommand command, CancellationToken cancellationToken)
    {
        var departmentResult = await _departmentsRepository.GetById(command.DepartmentId, cancellationToken);
        if (departmentResult.IsFailure)
            return departmentResult.Error.ToErrors();

        var locationResult = await _locationsRepository.GetById(command.LocationId, cancellationToken);
        if (locationResult.IsFailure)
            return locationResult.Error.ToErrors();
        
        bool exists = await _departmentLocationsRepository.DepLocRelationExists(command.DepartmentId, command.LocationId, cancellationToken);
        if (exists)
            return Error.Conflict("departmentLocations.relation.already_exists", $"Связь между Подразделением {command.DepartmentId} и Локацией {command.LocationId} уже существует").ToErrors();

        var relationResult = await _departmentLocationsRepository.AddRelation(
            command.DepartmentId,
            command.LocationId,
            cancellationToken);
            
        if (relationResult.IsFailure)
            return relationResult.Error.ToErrors();
        
        _logger.LogInformation("Связь между Подразделением {DepartmentId} и Локацией {LocationId} успешно создана", command.DepartmentId, command.LocationId);
        
        return UnitResult.Success<ErrorsList>();
    }
    
}