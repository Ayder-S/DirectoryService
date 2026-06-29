using DS.Application.Interfaces.Abstractions;

namespace DS.Application.Features.DepartmentLocations.DeleteRelation;

public record DeleteDepartmentLocationCommand(Guid DepartmentId, Guid LocationId) : ICommand;