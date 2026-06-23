using CSharpFunctionalExtensions;
using DS.Application.Abstractions;
using DS.Application.Commands.DepartmentLocation;
using DS.Application.Database;
using Microsoft.Extensions.Logging;
using Shared.AppFails;

namespace DS.Application.DepartmentLocations;

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
        
        return UnitResult.Success<ErrorsList>();
    }
}