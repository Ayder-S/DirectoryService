using DS.Application.Interfaces.Abstractions;

namespace DS.Application.Features.DepartmentLocations.CreateRelation;

public record CreateDepartmentLocationCommand(Guid DepartmentId, Guid LocationId) : ICommand;