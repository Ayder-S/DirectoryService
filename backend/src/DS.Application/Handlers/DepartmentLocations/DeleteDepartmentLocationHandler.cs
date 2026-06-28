using CSharpFunctionalExtensions;
using DS.Application.Commands.DepartmentLocation;
using DS.Application.Interfaces.Abstractions;
using DS.Application.Interfaces.Database;
using Microsoft.Extensions.Logging;
using Shared.Kernel.AppFails;

namespace DS.Application.Handlers.DepartmentLocations;

public class DeleteDepartmentLocationHandler : ICommandHandler<DeleteDepartmentLocationCommand>
{
    private readonly IDepartmentLocationsRepository _departmentLocationsRepository;
    private readonly ILogger<DeleteDepartmentLocationHandler> _logger;

    public DeleteDepartmentLocationHandler(
        IDepartmentLocationsRepository departmentLocationsRepository, 
        ILogger<DeleteDepartmentLocationHandler> logger)
    {
        _departmentLocationsRepository = departmentLocationsRepository;
        _logger = logger;
    }

    public async Task<UnitResult<ErrorsList>> Handle(
        DeleteDepartmentLocationCommand command,
        CancellationToken cancellationToken)
    {
        var deleteRelationResult =
            await _departmentLocationsRepository.DeleteRelation(command.DepartmentId, command.LocationId, cancellationToken);
        if (deleteRelationResult.IsFailure)
            return deleteRelationResult.Error.ToErrors();
        
        _logger.LogInformation("Связь между Подразделением {DepartmentId} и Локацией {LocationId} успешно удалена", command.DepartmentId, command.LocationId);
        
        return UnitResult.Success<ErrorsList>();
    }
}